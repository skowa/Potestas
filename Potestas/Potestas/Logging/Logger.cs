using System;
using NLog;

namespace Potestas.Logging
{
	public class Logger : ILogger
	{
		private readonly NLog.ILogger _logger;

		public Logger()
		{
			_logger = LogManager.GetCurrentClassLogger();
		}

		public void Info(string message) => _logger.Info(message);

		public void Warn(string message) => _logger.Warn(message);

		public void Error(string message, Exception exception) => _logger.Error(exception, message);
	}
}