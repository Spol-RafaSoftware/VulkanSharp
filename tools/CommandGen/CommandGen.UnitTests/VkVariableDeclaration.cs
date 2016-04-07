using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace CommandGen
{
	public class VkVariableDeclaration : IVkMethodImplementation
	{
		public VkFunctionArgument Source { get; set; }

		#region IVkMethodImplementation implementation
		public string GetString ()
		{
			throw new NotImplementedException ();
		}
		#endregion		
	}
}

