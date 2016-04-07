using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace CommandGen
{
	public class VkCommandParser
	{
		public VkCommandParser (Dictionary<string, string> translation)
		{
			mTypesTranslation = translation;
		}

		Dictionary<string, string> mTypesTranslation = new Dictionary<string, string> ();

		Dictionary<string, string> extensions = new Dictionary<string, string> {
			{ "EXT", "Ext" },
			{ "IMG", "Img" },
			{ "KHR", "Khr" }
		};

		// TODO: validate this mapping
		Dictionary<string, string> basicTypesMap = new Dictionary<string, string> {
			{ "int32_t", "Int32" },
			{ "uint32_t", "UInt32" },
			{ "uint64_t", "UInt64" },
			{ "uint8_t", "Byte" },
			{ "size_t", "UIntPtr" },
			{ "xcb_connection_t", "IntPtr" },
			{ "xcb_window_t", "IntPtr" },
		};

		string GetTypeCsName (string name, string typeName = "type")
		{
			if (mTypesTranslation.ContainsKey (name))
				return mTypesTranslation [name];

			string csName;

			if (name.StartsWith ("Vk" , StringComparison.InvariantCulture))
				csName = name.Substring (2);
			else if (name.EndsWith ("_t", StringComparison.InvariantCulture))
			{
				if (!basicTypesMap.ContainsKey (name))
					throw new NotImplementedException (string.Format ("Mapping for the basic type {0} isn't supported", name));

				csName = basicTypesMap[name];
			}
			else
			{
				Console.WriteLine ("warning: {0} name '{1}' doesn't start with Vk prefix or end with _t suffix", typeName, name);
				csName = name;
			}

			foreach (var ext in extensions)
				if (csName.EndsWith (ext.Key, StringComparison.InvariantCulture))
					csName = csName.Substring (0, csName.Length - ext.Value.Length) + ext.Value;

			return csName;
		}

		void ParseNativeInterface (XElement top, VkCommandInfo result)
		{
			var function = new VkNativeInterface ();
			function.Name = result.Name;
			function.ReturnType = result.CsReturnType;

			int index = 0;
			foreach (var param in top.Elements ("param"))
			{
				var arg = new VkFunctionArgument {
					Index = index
				};
				var tokens = param.Value.Split (new[] {
					' ',
					'[',
					']'
				}, StringSplitOptions.RemoveEmptyEntries);
				arg.IsFixedArray = false;
				if (tokens.Length == 2)
				{
					// usually instance
					arg.ArgumentCppType = tokens [0];
					arg.IsConst = false;
					// or int
				}
				else if (tokens.Length == 3)
				{
					// possible const pointer
					arg.IsConst = (tokens [0] == "const");
					arg.ArgumentCppType = tokens [1];
				}
				else if (tokens.Length == 4)
				{
					// const float array
					arg.IsConst = (tokens [0] == "const");
					arg.ArgumentCppType = tokens [1];
					arg.ArrayConstant = tokens [3];
					arg.IsFixedArray = true;
				}
				arg.IsPointer = arg.ArgumentCppType.IndexOf ('*') >= 0;

				arg.Name = param.Element ("name").Value;
				arg.BaseCppType = param.Element ("type").Value;
				arg.BaseCsType = GetTypeCsName (arg.BaseCppType);
				XAttribute optionalAttr = param.Attribute ("optional");
				arg.IsOptional = optionalAttr != null && optionalAttr.Value == "true";
				XAttribute lengthAttr = param.Attribute ("len");
				if (lengthAttr != null)
				{
					var lengths = lengthAttr.Value.Split (',').Where (s => s != "null-terminated").ToArray ();
					if (lengths.Length != 1)
					{
						throw new NotImplementedException (string.Format ("param.len != 1 ({0} found)", lengths.Length));
					}
					arg.LengthVariable = lengths [0];
				}
				function.Arguments.Add (arg);

				// DETERMINE CSHARP TYPE 
				if (arg.ArgumentCppType == "char*")
				{
					arg.ArgumentCsType = "string";
				} else if (arg.ArgumentCppType == "void*")
				{
					arg.ArgumentCsType = "IntPtr";
				} 
				else 
				{
					HandleInfo found;
					if (Handles.TryGetValue(arg.BaseCsType, out found))
					{
						arg.ArgumentCsType = (found.type == "VK_DEFINE_HANDLE") ? "IntPtr" : "UInt64";
					}
					else
					{
						arg.ArgumentCsType = arg.BaseCsType;
					}
				}	

				arg.UseOut = !arg.IsConst && arg.IsPointer;

				++index;
			}
			result.NativeFunction = function;
		}

		public Dictionary<string, HandleInfo> Handles = new Dictionary<string, HandleInfo> ();
		public HashSet<string> blittableTypes = new HashSet<string> ();

		void ParseMethodSignature (XElement top, VkCommandInfo result)
		{
			var signature = new VkMethodSignature ();
			signature.Name = (result.Name.StartsWith ("vk", StringComparison.InvariantCulture)) ? result.Name.Substring (2) : result.Name;
			signature.ReturnType = result.CsReturnType;

			var arguments = new Dictionary<string, VkFunctionArgument> ();
			foreach (var arg in result.NativeFunction.Arguments)
			{
				arguments.Add (arg.Name, arg);
			}

			// FILTER OUT VALUES FOR SIGNATURE
			foreach (var arg in result.NativeFunction.Arguments)
			{
				// object reference
				if (Handles.ContainsKey(arg.BaseCsType) && ! arg.IsPointer)
				{
					if (arg.Index == 0)
					{
						result.FirstInstance = arg;
					}

					arguments.Remove (arg.Name);
				}

				VkFunctionArgument localLength;
				if (arg.LengthVariable != null && arguments.TryGetValue (arg.LengthVariable, out localLength))
				{
					result.LocalVariables.Add (localLength);
					arguments.Remove (arg.LengthVariable);
				}
			}

			signature.Parameters = (from arg in arguments.Values
			                        orderby arg.Index ascending
			                        select new VkMethodParameter{ Name = arg.Name, Source = arg }).ToList ();

			foreach (var param in signature.Parameters)
			{				
				param.CsType = param.Source.BaseCsType;
				param.UseOut = param.Source.UseOut;
				param.IsFixedArray = param.Source.IsFixedArray;
				param.IsArrayParameter = !param.Source.IsConst && param.Source.LengthVariable != null;
				param.IsNullableType = param.Source.IsPointer && param.Source.IsOptional;
				param.UseRef = param.Source.UseOut && blittableTypes.Contains (param.CsType);
			}

			result.MethodSignature = signature;
		}

		void ParseFunctionCalls (XElement top, VkCommandInfo result)
		{
			//throw new NotImplementedException ();

			int noOfOuts = 0;
			int noOfArguments = result.NativeFunction.Arguments.Count;
			for (int i = 0; i < noOfArguments; ++i)
			{
				var arg = result.NativeFunction.Arguments [i];
				if (arg.UseOut)
				{
					++noOfOuts;
				}
			}

			if (result.LocalVariables.Count > 0)
			{
				foreach (var local in result.LocalVariables)
				{
					var letVar = new VkVariableDeclaration{ Source = local };
					result.Lines.Add (letVar);
				}

				var fetch = new VkFunctionCall{ Call = result.NativeFunction };
				result.Calls.Add (fetch);
				// method call
				result.Lines.Add (fetch);

				if (result.NativeFunction.ReturnType != "void")
				{
					var errorCheck = new VkConditionalCheck{ReturnType=result.NativeFunction.ReturnType};
					result.Lines.Add (errorCheck);
				}

				foreach (var arg in result.NativeFunction.Arguments)
				{
					var item = new VkCallArgument{Source = arg };
					item.IsNull = (arg.UseOut && arg.LengthVariable != null);
					fetch.Arguments.Add (item);
				}
			}

			foreach (var param in result.MethodSignature.Parameters)
			{
				if (param.UseOut)
				{
					var letVar = new VkVariableDeclaration{ Source = param.Source };
					result.Lines.Add (letVar);
				}
			}

			var body = new VkFunctionCall{ Call = result.NativeFunction };
			result.Calls.Add (body);
			foreach (var arg in result.NativeFunction.Arguments)
			{
				var item = new VkCallArgument{Source = arg };
				item.IsNull = (arg.UseOut && arg.LengthVariable != null);
				body.Arguments.Add (item);
			}

			result.Lines.Add (body);
		}

		public VkCommandInfo Parse (XElement top)
		{
			var result = new VkCommandInfo ();
			var protoElem = top.Element ("proto");

			result.Name = protoElem.Element ("name").Value;
			result.CppReturnType = protoElem.Element ("type").Value;
			result.CsReturnType = GetTypeCsName (result.CppReturnType);

			ParseNativeInterface (top, result);

			ParseMethodSignature (top, result);

			ParseFunctionCalls (top, result);

			return result;
		}
	}
}

