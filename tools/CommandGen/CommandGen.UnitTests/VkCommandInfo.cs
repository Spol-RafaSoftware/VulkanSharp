using System;
using System.Collections.Generic;

namespace CommandParser
{
	public class VkCommandInfo
	{
		public VkCommandInfo ()
		{
			LocalVariables = new List<VkFunctionArgument> ();
		}

		public string CsReturnType {
			get;
			set;
		}

		public string CppReturnType {
			get;
			set;
		}

		public string Name { get; set; }

		public VkFunctionArgument FirstInstance { get; set; }
		public List<VkFunctionArgument> LocalVariables { get; set; }
		public VkNativeInterface NativeFunction { get; set; }
		public VkMethodSignature MethodSignature { get; set; }
	}
}

