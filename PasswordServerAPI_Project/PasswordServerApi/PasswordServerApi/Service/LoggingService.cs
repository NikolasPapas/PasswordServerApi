using Serilog;
using PasswordServerApi.Interfaces;
using Microsoft.Extensions.Logging;

namespace PasswordServerApi.Service
{
	public class LoggingService : ILoggingService
	{
		private const string LoggerMessage = "PasswordServerApiLog:";
		readonly ILogger<LoggingService> _logger;

		public LoggingService(ILogger<LoggingService> logger)
		{

			_logger = logger;
		}

		public void LogInfo(string message)
		{
			_logger.LogWarning($"{LoggerMessage} {message}");
		}

		public void LogWarning(string message)
		{
			_logger.LogError($"{LoggerMessage} {message}");
		}
		public void LogCritical(string message)
		{
			_logger.LogCritical($"Critical {LoggerMessage} {message}");
		}
	}
}
