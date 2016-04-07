using System;
using System.Collections.Generic;
using CommandGen;

namespace CommandGen
{
	public class VkFunctionCall : IVkMethodImplementation
	{
		public VkFunctionCall ()
		{
			Arguments = new List<VkCallArgument> ();
		}

		public VkNativeInterface Call { get; set; }

		#region IVkMethodImplementation implementation


		public string GetString ()
		{
			throw new NotImplementedException ();
		}


		#endregion

		public List<VkCallArgument> Arguments { get; set; }
	}
}

