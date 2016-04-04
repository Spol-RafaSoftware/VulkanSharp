using NUnit.Framework;
using System;
using System.Xml.Linq;
using CommandParser;
using System.Collections.Generic;

namespace AddressOf.UnitTests
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public void TestCase1 ()
		{
			const string xml = @"
		        <command successcodes=""VK_SUCCESS"" errorcodes=""VK_ERROR_OUT_OF_HOST_MEMORY,VK_ERROR_OUT_OF_DEVICE_MEMORY,VK_ERROR_INITIALIZATION_FAILED,VK_ERROR_LAYER_NOT_PRESENT,VK_ERROR_EXTENSION_NOT_PRESENT,VK_ERROR_INCOMPATIBLE_DRIVER"">
		            <proto><type>VkResult</type> <name>vkCreateInstance</name></proto>
		            <param>const <type>VkInstanceCreateInfo</type>* <name>pCreateInfo</name></param>
		            <param optional=""true"">const <type>VkAllocationCallbacks</type>* <name>pAllocator</name></param>
		            <param><type>VkInstance</type>* <name>pInstance</name></param>
		        </command>";

			XElement top = XElement.Parse (xml, LoadOptions.PreserveWhitespace);
			var parser = new VkCommandParser (new Dictionary<string, string>());

			parser.Handles.Add("Instance", new HandleInfo{ name="Instance", type="VK_DEFINE_HANDLE"});
			Assert.AreEqual (1, parser.Handles.Keys.Count);

			VkCommandInfo command = parser.Parse (top);
			Assert.IsNotNull (command);
			Assert.AreEqual ("vkCreateInstance", command.Name);
			Assert.AreEqual ("VkResult", command.CppReturnType);
			Assert.AreEqual ("Result",   command.CsReturnType);
			Assert.IsNull (command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull (function, "command.NativeFunction is not null");
			Assert.AreEqual ("Result", function.ReturnType);
			Assert.AreEqual (3, function.Arguments.Count);

			{
				const int index = 0;
				var arg_0 = function.Arguments [index];
				Assert.IsNotNull (arg_0);
				Assert.AreEqual (index, arg_0.Index);
				Assert.AreEqual ("pCreateInfo", arg_0.Name);
				Assert.AreEqual ("VkInstanceCreateInfo", arg_0.BaseCppType);
				Assert.AreEqual ("InstanceCreateInfo", arg_0.BaseCsType);
				Assert.AreEqual ("VkInstanceCreateInfo*", arg_0.ArgumentCppType);
				Assert.AreEqual ("InstanceCreateInfo", arg_0.ArgumentCsType);
				Assert.IsTrue   (arg_0.IsConst);
				Assert.IsFalse  (arg_0.IsOptional);
				Assert.IsFalse  (arg_0.IsFixedArray);
				Assert.IsNull   (arg_0.LengthVariable);
				Assert.IsNull   (arg_0.ArrayConstant);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments [index];
				Assert.IsNotNull (arg_1);
				Assert.AreEqual (index, arg_1.Index);
				Assert.AreEqual ("pAllocator", arg_1.Name);
				Assert.AreEqual ("VkAllocationCallbacks", arg_1.BaseCppType);
				Assert.AreEqual ("AllocationCallbacks", arg_1.BaseCsType);
				Assert.AreEqual ("VkAllocationCallbacks*", arg_1.ArgumentCppType);
				Assert.AreEqual ("AllocationCallbacks", arg_1.ArgumentCsType);
				Assert.IsTrue   (arg_1.IsConst);
				Assert.IsTrue   (arg_1.IsOptional);
				Assert.IsFalse  (arg_1.IsFixedArray);
				Assert.IsNull   (arg_1.LengthVariable);
				Assert.IsNull   (arg_1.ArrayConstant);
			}

			{
				const int index = 2;
				var arg_2 = function.Arguments [index];
				Assert.IsNotNull (arg_2);
				Assert.AreEqual (index, arg_2.Index);
				Assert.AreEqual ("pInstance", arg_2.Name);
				Assert.AreEqual ("VkInstance", arg_2.BaseCppType);
				Assert.AreEqual ("Instance", arg_2.BaseCsType);
				Assert.AreEqual ("VkInstance*", arg_2.ArgumentCppType);
				Assert.AreEqual ("IntPtr", arg_2.ArgumentCsType);
				Assert.IsFalse  (arg_2.IsConst);
				Assert.IsFalse  (arg_2.IsOptional);
				Assert.IsFalse  (arg_2.IsFixedArray);
				Assert.IsNull   (arg_2.LengthVariable);
				Assert.IsNull   (arg_2.ArrayConstant);
			}

			// METHOD SIGNATURE

			var method = command.MethodSignature;
			Assert.IsNotNull (method, "command.MethodSignature is not null");
			Assert.AreEqual  ("CreateInstance", method.Name);
			Assert.AreEqual  ("Result",   method.ReturnType);
			Assert.AreEqual  (3, method.Parameters.Count);

			{
				const int index = 0;
				var param_0 = method.Parameters [index];
				Assert.IsNotNull (param_0);
				Assert.AreEqual (0, param_0.Source.Index);
				Assert.AreEqual ("pCreateInfo", param_0.Name);
				Assert.IsFalse  (param_0.IsNullableType);
				Assert.IsFalse (param_0.UseOut);
				Assert.AreEqual ("InstanceCreateInfo", param_0.CsType);
				Assert.IsFalse (param_0.UseRef);
				Assert.IsFalse (param_0.IsFixedArray);
			}

			{
				const int index = 1;
				var param_1 = method.Parameters [index];
				Assert.IsNotNull (param_1);
				Assert.AreEqual (1, param_1.Source.Index);
				Assert.AreEqual ("pAllocator", param_1.Name);
				Assert.IsTrue  (param_1.IsNullableType);
				Assert.IsFalse (param_1.UseOut);
				Assert.AreEqual ("AllocationCallbacks", param_1.CsType);
				Assert.IsFalse (param_1.UseRef);
				Assert.IsFalse (param_1.IsFixedArray);
				Assert.IsFalse (param_1.IsArray);
			}

			{
				const int index = 2;
				var param_1 = method.Parameters [index];
				Assert.IsNotNull (param_1);
				Assert.AreEqual (2, param_1.Source.Index);
				Assert.AreEqual ("pInstance", param_1.Name);
				Assert.IsFalse  (param_1.IsNullableType);
				Assert.IsTrue (param_1.UseOut);
				Assert.AreEqual ("Instance", param_1.CsType);
				Assert.IsFalse (param_1.UseRef);
				Assert.IsFalse (param_1.IsFixedArray);
				Assert.IsFalse (param_1.IsArray);
			}
		}

		[Test ()]
		public void TestCase2 ()
		{
			const string xml = @"
		        <command successcodes=""VK_SUCCESS,VK_INCOMPLETE"" errorcodes=""VK_ERROR_OUT_OF_HOST_MEMORY,VK_ERROR_OUT_OF_DEVICE_MEMORY,VK_ERROR_INITIALIZATION_FAILED"">
		            <proto><type>VkResult</type> <name>vkEnumeratePhysicalDevices</name></proto>
		            <param><type>VkInstance</type> <name>instance</name></param>
		            <param optional=""false,true""><type>uint32_t</type>* <name>pPhysicalDeviceCount</name></param>
		            <param optional=""true"" len=""pPhysicalDeviceCount""><type>VkPhysicalDevice</type>* <name>pPhysicalDevices</name></param>
		        </command>";

			XElement top = XElement.Parse (xml, LoadOptions.PreserveWhitespace);
			var parser = new VkCommandParser (new Dictionary<string, string>());

			parser.Handles.Add("PhysicalDevice", new HandleInfo{ name="PhysicalDevice", type="VK_DEFINE_HANDLE"});
			parser.Handles.Add("Instance", new HandleInfo{ name="Instance", type="VK_DEFINE_HANDLE"});
			Assert.AreEqual (2, parser.Handles.Keys.Count);

			VkCommandInfo command = parser.Parse (top);
			Assert.IsNotNull (command);
			Assert.AreEqual ("vkEnumeratePhysicalDevices", command.Name);
			Assert.AreEqual ("VkResult", command.CppReturnType);
			Assert.AreEqual ("Result",   command.CsReturnType);
			Assert.IsNotNull (command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull (function, "command.NativeFunction is not null");
			Assert.AreEqual ("Result", function.ReturnType);
			Assert.AreEqual (3, function.Arguments.Count);

			{
				const int index = 0;
				var arg_0 = function.Arguments [index];
				Assert.IsNotNull (arg_0);
				Assert.AreEqual (index, arg_0.Index);
				Assert.AreEqual ("instance", arg_0.Name);
				Assert.AreEqual ("VkInstance", arg_0.BaseCppType);
				Assert.AreEqual ("Instance", arg_0.BaseCsType);
				Assert.AreEqual ("VkInstance", arg_0.ArgumentCppType);
				Assert.AreEqual ("IntPtr", arg_0.ArgumentCsType);
				Assert.IsFalse  (arg_0.IsConst);
				Assert.IsFalse  (arg_0.IsOptional);
				Assert.IsFalse  (arg_0.IsFixedArray);
				Assert.IsNull   (arg_0.LengthVariable);
				Assert.IsNull   (arg_0.ArrayConstant);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments [index];
				Assert.IsNotNull (arg_1);
				Assert.AreEqual (index, arg_1.Index);
				Assert.AreEqual ("pPhysicalDeviceCount", arg_1.Name);
				Assert.AreEqual ("uint32_t", arg_1.BaseCppType);
				Assert.AreEqual ("UInt32", arg_1.BaseCsType);
				Assert.AreEqual ("uint32_t*", arg_1.ArgumentCppType);
				Assert.AreEqual ("UInt32", arg_1.ArgumentCsType);
				Assert.IsFalse  (arg_1.IsConst);
				Assert.IsFalse  (arg_1.IsOptional);
				Assert.IsFalse  (arg_1.IsFixedArray);
				Assert.IsNull   (arg_1.LengthVariable);
				Assert.IsNull   (arg_1.ArrayConstant);
			}

			{
				const int index = 2;
				var arg_2 = function.Arguments [index];
				Assert.IsNotNull (arg_2);
				Assert.AreEqual (index, arg_2.Index);
				Assert.AreEqual ("pPhysicalDevices", arg_2.Name);
				Assert.AreEqual ("VkPhysicalDevice", arg_2.BaseCppType);
				Assert.AreEqual ("PhysicalDevice", arg_2.BaseCsType);
				Assert.AreEqual ("VkPhysicalDevice*", arg_2.ArgumentCppType);
				Assert.AreEqual ("IntPtr", arg_2.ArgumentCsType);
				Assert.IsFalse  (arg_2.IsConst);
				Assert.IsTrue   (arg_2.IsOptional);
				Assert.IsFalse  (arg_2.IsFixedArray);
				Assert.AreEqual ("pPhysicalDeviceCount", arg_2.LengthVariable);
				Assert.IsNull   (arg_2.ArrayConstant);
			}

			// METHOD SIGNATURE
			var method = command.MethodSignature;
			Assert.IsNotNull (method, "command.MethodSignature is not null");
			Assert.AreEqual ("EnumeratePhysicalDevices", method.Name);
			Assert.AreEqual ("Result", method.ReturnType);
			Assert.AreEqual (1, method.Parameters.Count);

			{
				const int index = 0;
				var param_0 = method.Parameters [index];
				Assert.IsNotNull (param_0);
				Assert.AreEqual (2, param_0.Source.Index);
				Assert.AreEqual ("pPhysicalDevices", param_0.Name);
				Assert.IsTrue   (param_0.IsNullableType);
				Assert.IsTrue   (param_0.UseOut);
				Assert.AreEqual ("PhysicalDevice", param_0.CsType);
				Assert.IsFalse (param_0.UseRef);
				Assert.IsFalse (param_0.IsFixedArray);
				Assert.IsTrue  (param_0.IsArray);
			}
		}

		[Test ()]
		public void TestCase3 ()
		{
			const string xml = @"
		        <command successcodes=""VK_SUCCESS,VK_INCOMPLETE"" errorcodes=""VK_ERROR_OUT_OF_HOST_MEMORY,VK_ERROR_OUT_OF_DEVICE_MEMORY"">
		            <proto><type>VkResult</type> <name>vkEnumerateDeviceLayerProperties</name></proto>
		            <param optional=""false,true""><type>VkPhysicalDevice</type> <name>physicalDevice</name></param>
		            <param optional=""false,true""><type>uint32_t</type>* <name>pPropertyCount</name></param>
		            <param optional=""true"" len=""pPropertyCount""><type>VkLayerProperties</type>* <name>pProperties</name></param>
		        </command>";

			XElement top = XElement.Parse (xml, LoadOptions.PreserveWhitespace);
			var parser = new VkCommandParser (new Dictionary<string, string>());

			parser.Handles.Add("PhysicalDevice", new HandleInfo{ name="PhysicalDevice", type="VK_DEFINE_HANDLE"});
			Assert.AreEqual (1, parser.Handles.Keys.Count);

			VkCommandInfo command = parser.Parse (top);
			Assert.IsNotNull (command);
			Assert.AreEqual ("vkEnumerateDeviceLayerProperties", command.Name);
			Assert.AreEqual ("VkResult", command.CppReturnType);
			Assert.AreEqual ("Result",   command.CsReturnType);
			Assert.IsNotNull (command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull (function, "command.NativeFunction is not null");
			Assert.AreEqual ("Result", function.ReturnType);
			Assert.AreEqual (3, function.Arguments.Count);

			{
				const int index = 0;
				var arg_0 = function.Arguments [index];
				Assert.IsNotNull (arg_0);
				Assert.AreEqual (index, arg_0.Index);
				Assert.AreEqual ("physicalDevice", arg_0.Name);
				Assert.AreEqual ("VkPhysicalDevice", arg_0.BaseCppType);
				Assert.AreEqual ("PhysicalDevice", arg_0.BaseCsType);
				Assert.AreEqual ("VkPhysicalDevice", arg_0.ArgumentCppType);
				Assert.AreEqual ("IntPtr", arg_0.ArgumentCsType);
				Assert.IsFalse  (arg_0.IsConst);
				Assert.IsFalse  (arg_0.IsOptional);
				Assert.IsFalse  (arg_0.IsFixedArray);
				Assert.IsNull   (arg_0.LengthVariable);
				Assert.IsNull   (arg_0.ArrayConstant);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments [index];
				Assert.IsNotNull (arg_1);
				Assert.AreEqual (index, arg_1.Index);
				Assert.AreEqual ("pPropertyCount", arg_1.Name);
				Assert.AreEqual ("uint32_t", arg_1.BaseCppType);
				Assert.AreEqual ("UInt32", arg_1.BaseCsType);
				Assert.AreEqual ("uint32_t*", arg_1.ArgumentCppType);
				Assert.AreEqual ("UInt32", arg_1.ArgumentCsType);
				Assert.IsFalse  (arg_1.IsConst);
				Assert.IsFalse  (arg_1.IsOptional);
				Assert.IsFalse  (arg_1.IsFixedArray);
				Assert.IsNull   (arg_1.LengthVariable);
				Assert.IsNull   (arg_1.ArrayConstant);
			}

			{
				const int index = 2;
				var arg_2 = function.Arguments [index];
				Assert.IsNotNull (arg_2);
				Assert.AreEqual (index, arg_2.Index);
				Assert.AreEqual ("pProperties", arg_2.Name);
				Assert.AreEqual ("VkLayerProperties", arg_2.BaseCppType);
				Assert.AreEqual ("LayerProperties", arg_2.BaseCsType);
				Assert.AreEqual ("VkLayerProperties*", arg_2.ArgumentCppType);
				Assert.AreEqual ("LayerProperties", arg_2.ArgumentCsType);
				Assert.IsFalse  (arg_2.IsConst);
				Assert.IsTrue   (arg_2.IsOptional);
				Assert.IsFalse  (arg_2.IsFixedArray);
				Assert.AreEqual ("pPropertyCount", arg_2.LengthVariable);
				Assert.IsNull   (arg_2.ArrayConstant);
			}

			// METHOD SIGNATURE
			var method = command.MethodSignature;
			Assert.IsNotNull (method, "command.MethodSignature is not null");
			Assert.AreEqual ("Result", method.ReturnType);
			Assert.AreEqual (1, method.Parameters.Count);

			{
				const int index = 0;
				var param_0 = method.Parameters [index];
				Assert.IsNotNull (param_0);
				Assert.AreEqual (2, param_0.Source.Index);
				Assert.AreEqual ("pProperties", param_0.Name);
				Assert.IsTrue   (param_0.IsNullableType);
				Assert.IsTrue   (param_0.UseOut);
				Assert.AreEqual ("LayerProperties", param_0.CsType);
				Assert.IsFalse (param_0.UseRef);
				Assert.IsFalse (param_0.IsFixedArray);
				Assert.IsTrue  (param_0.IsArray);
			}
		}

		[Test ()]
		public void TestCase4 ()
		{
			const string xml = @"
		        <command successcodes=""VK_SUCCESS,VK_INCOMPLETE"" errorcodes=""VK_ERROR_OUT_OF_HOST_MEMORY,VK_ERROR_OUT_OF_DEVICE_MEMORY"">
		            <proto><type>VkResult</type> <name>vkEnumerateInstanceLayerProperties</name></proto>
		            <param optional=""false,true""><type>uint32_t</type>* <name>pPropertyCount</name></param>
		            <param optional=""true"" len=""pPropertyCount""><type>VkLayerProperties</type>* <name>pProperties</name></param>
		        </command>";

			XElement top = XElement.Parse (xml, LoadOptions.PreserveWhitespace);
			var parser = new VkCommandParser (new Dictionary<string, string>());

			Assert.AreEqual (0, parser.Handles.Keys.Count);

			VkCommandInfo command = parser.Parse (top);
			Assert.IsNotNull (command);
			Assert.AreEqual ("vkEnumerateInstanceLayerProperties", command.Name);
			Assert.AreEqual ("VkResult", command.CppReturnType);
			Assert.AreEqual ("Result",   command.CsReturnType);
			Assert.IsNull (command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull (function, "command.NativeFunction is not null");
			Assert.AreEqual ("Result", function.ReturnType);
			Assert.AreEqual (2, function.Arguments.Count);

			{
				const int index = 0;
				var arg_0 = function.Arguments [index];
				Assert.IsNotNull (arg_0);
				Assert.AreEqual (index, arg_0.Index);
				Assert.AreEqual ("pPropertyCount", arg_0.Name);
				Assert.AreEqual ("uint32_t", arg_0.BaseCppType);
				Assert.AreEqual ("UInt32", arg_0.BaseCsType);
				Assert.AreEqual ("uint32_t*", arg_0.ArgumentCppType);
				Assert.AreEqual ("UInt32", arg_0.ArgumentCsType);
				Assert.IsFalse  (arg_0.IsConst);
				Assert.IsFalse  (arg_0.IsOptional);
				Assert.IsFalse  (arg_0.IsFixedArray);
				Assert.IsNull   (arg_0.LengthVariable);
				Assert.IsNull   (arg_0.ArrayConstant);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments [index];
				Assert.IsNotNull (arg_1);
				Assert.AreEqual (index, arg_1.Index);
				Assert.AreEqual ("pProperties", arg_1.Name);
				Assert.AreEqual ("VkLayerProperties", arg_1.BaseCppType);
				Assert.AreEqual ("LayerProperties", arg_1.BaseCsType);
				Assert.AreEqual ("VkLayerProperties*", arg_1.ArgumentCppType);
				Assert.AreEqual ("LayerProperties", arg_1.ArgumentCsType);
				Assert.IsFalse  (arg_1.IsConst);
				Assert.IsTrue   (arg_1.IsOptional);
				Assert.IsFalse  (arg_1.IsFixedArray);
				Assert.AreEqual ("pPropertyCount", arg_1.LengthVariable);
				Assert.IsNull   (arg_1.ArrayConstant);
			}

			// METHOD SIGNATURE
			var method = command.MethodSignature;
			Assert.IsNotNull (method, "command.MethodSignature is not null");
			Assert.AreEqual ("Result", method.ReturnType);
			Assert.AreEqual (1, method.Parameters.Count);
		}

		[Test ()]
		public void TestCase5 ()
		{
			const string xml = @"
		        <command queues=""graphics"" renderpass=""both"" cmdbufferlevel=""primary,secondary"">
		            <proto><type>void</type> <name>vkCmdSetBlendConstants</name></proto>
		            <param externsync=""true""><type>VkCommandBuffer</type> <name>commandBuffer</name></param>
		            <param>const <type>float</type> <name>blendConstants</name>[4]</param>
		        </command>";

			XElement top = XElement.Parse (xml, LoadOptions.PreserveWhitespace);
			var parser = new VkCommandParser (new Dictionary<string, string>());

			parser.Handles.Add("CommandBuffer", new HandleInfo{ name="CommandBuffer", type="VK_DEFINE_HANDLE"});
			Assert.AreEqual (1, parser.Handles.Keys.Count);

			VkCommandInfo command = parser.Parse (top);
			Assert.IsNotNull (command);
			Assert.AreEqual ("vkCmdSetBlendConstants", command.Name);
			Assert.AreEqual ("void", command.CppReturnType);
			Assert.AreEqual ("void", command.CsReturnType);
			Assert.IsNotNull (command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull (function, "command.NativeFunction is not null");
			Assert.AreEqual ("void", function.ReturnType);
			Assert.AreEqual (2, function.Arguments.Count);

			{
				const int index = 0;
				var arg_0 = function.Arguments [index];
				Assert.IsNotNull (arg_0);
				Assert.AreEqual (index, arg_0.Index);
				Assert.AreEqual ("commandBuffer", arg_0.Name);
				Assert.AreEqual ("VkCommandBuffer", arg_0.BaseCppType);
				Assert.AreEqual ("CommandBuffer", arg_0.BaseCsType);
				Assert.AreEqual ("VkCommandBuffer", arg_0.ArgumentCppType);
				Assert.AreEqual ("IntPtr", arg_0.ArgumentCsType);
				Assert.IsFalse  (arg_0.IsConst);
				Assert.IsFalse  (arg_0.IsOptional);
				Assert.IsFalse  (arg_0.IsFixedArray);
				Assert.IsNull   (arg_0.LengthVariable);
				Assert.IsNull   (arg_0.ArrayConstant);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments [index];
				Assert.IsNotNull (arg_1);
				Assert.AreEqual (index, arg_1.Index);
				Assert.AreEqual ("blendConstants", arg_1.Name);
				Assert.AreEqual ("float", arg_1.BaseCppType);
				Assert.AreEqual ("float", arg_1.BaseCsType);
				Assert.AreEqual ("float", arg_1.ArgumentCppType);
				Assert.AreEqual ("float", arg_1.ArgumentCsType);
				Assert.IsTrue   (arg_1.IsConst);
				Assert.IsFalse  (arg_1.IsOptional);
				Assert.IsTrue   (arg_1.IsFixedArray);
				Assert.IsNull   (arg_1.LengthVariable);
				Assert.AreEqual ("4", arg_1.ArrayConstant);
			}

			// METHOD SIGNATURE
			var method = command.MethodSignature;
			Assert.IsNotNull (method, "command.MethodSignature is not null");
			Assert.AreEqual (1, method.Parameters.Count);
		}

		[Test ()]
		public void TestCase6 ()
		{
			const string xml = @"
	        <command successcodes=""VK_SUCCESS"" errorcodes=""VK_ERROR_OUT_OF_HOST_MEMORY,VK_ERROR_OUT_OF_DEVICE_MEMORY"">
	            <proto><type>VkResult</type> <name>vkCreateQueryPool</name></proto>
	            <param><type>VkDevice</type> <name>device</name></param>
	            <param>const <type>VkQueryPoolCreateInfo</type>* <name>pCreateInfo</name></param>
	            <param optional=""true"">const <type>VkAllocationCallbacks</type>* <name>pAllocator</name></param>
	            <param><type>VkQueryPool</type>* <name>pQueryPool</name></param>
	        </command>";

			XElement top = XElement.Parse (xml, LoadOptions.PreserveWhitespace);
			var parser = new VkCommandParser (new Dictionary<string, string>());

			parser.Handles.Add("CommandBuffer", new HandleInfo{ name="CommandBuffer", type="VK_DEFINE_HANDLE"});
			Assert.AreEqual (1, parser.Handles.Keys.Count);

			VkCommandInfo command = parser.Parse (top);
			Assert.IsNotNull (command);
		}
	}
}

