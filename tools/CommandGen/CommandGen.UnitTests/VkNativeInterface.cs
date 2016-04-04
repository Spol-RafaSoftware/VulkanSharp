using System;
using System.Collections.Generic;

namespace CommandParser
{
	public class VkNativeInterface
	{
		public VkNativeInterface ()
		{
			Arguments = new List<VkFunctionArgument> ();
			Attributes = new List<VkAttributeInfo> ();
		}

		public string Name {
			get;
			set;
		}

		public string ReturnType { get; set; }

		public List<VkAttributeInfo> Attributes {get;set;}

		public List<VkFunctionArgument> Arguments {get;set;}
	}
}

