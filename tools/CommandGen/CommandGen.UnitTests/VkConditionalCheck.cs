using System;

namespace CommandGen
{
	public class VkConditionalCheck : IVkMethodImplementation
	{
		public string ReturnType { get; set; }

		#region IVkMethodImplementation implementation
		public string GetString ()
		{
			throw new NotImplementedException ();
		}
		#endregion		
	}
}

