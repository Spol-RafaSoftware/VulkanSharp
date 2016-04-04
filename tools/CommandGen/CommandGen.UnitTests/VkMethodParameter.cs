using System;
using System.Collections.Generic;

namespace CommandParser
{
	public class VkMethodParameter
	{
		public bool UseRef {
			get;
			set;
		}

		public string CsType {
			get;
			set;
		}

		public bool IsNullableType {
			get;
			set;
		}

		public bool UseOut {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public bool IsFixedArray {
			get;
			set;
		}

		public bool IsArray {
			get;
			set;
		}

		public VkFunctionArgument Source { get; set; }
	}
}

