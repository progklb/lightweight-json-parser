using System;

namespace LightWeightJsonParser
{
	/// <summary>
	/// Custom exception to be used for LWJP errors.
	/// </summary>
    internal class LWJPException : Exception
    {
		public LWJPException()
		{
		}

		public LWJPException(string message)
			: base(message)
		{
		}

		public LWJPException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
