using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Interfaces
{
	public interface ILoggingService
	{
		void LogInfo(string message);

		void LogWarning(string message);

		void LogCritical(string message);
	}
}
