using System;

namespace tepixPetitions.common.Types.Exceptions
{
	public class BusinessException : Exception
	{
		public BusinessException(string message) : base(message)
		{

		}
	}
}
