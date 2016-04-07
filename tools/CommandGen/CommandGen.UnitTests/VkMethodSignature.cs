using System;
using System.Collections.Generic;

namespace CommandGen
{
	public class VkMethodSignature
	{
		public VkMethodSignature ()
		{
			Parameters = new List<VkMethodParameter> ();
		}

		public string Name {
			get;
			set;
		}

		public string ReturnType {
			get;
			set;
		}

		public List<VkMethodParameter> Parameters {get;set;}
	}
}

