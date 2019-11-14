using System;

namespace tepixPetitions.common.Types.Exceptions
{
	public class InvalidActionException : BusinessException
	{
		public InvalidActionException(string message) : base(message)
		{
		}
	}
}
