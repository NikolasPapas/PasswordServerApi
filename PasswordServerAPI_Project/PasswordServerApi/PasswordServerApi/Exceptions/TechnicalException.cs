using System;

namespace tepixPetitions.common.Types.Exceptions
{
	public class TechnicalException : Exception
	{
		public TechnicalException(string message, Exception ex) : base(message, ex)
		{

		}
	}
}
