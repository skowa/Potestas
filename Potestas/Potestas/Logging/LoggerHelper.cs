using System;
using System.Threading.Tasks;

namespace Potestas.Logging
{
	internal static class LoggerHelper
	{
		internal static T RunWithLogging<T>(ILogger logger, Func<T> func, string methodName)
		{
			try
			{
				logger.Info($"Start executing method {methodName}.");
				T result = func();
				logger.Info($"End executing method {methodName}.");

				return result;
			}
			catch (Exception ex)
			{
				logger.Error($"Error occured during running {methodName}", ex);
				throw;
			}
		}

		internal static void RunWithLogging(ILogger logger, Action action, string methodName)
		{
			try
			{
				logger.Info($"Start executing method {methodName}.");
				action();
				logger.Info($"End executing method {methodName}.");
			}
			catch (Exception ex)
			{
				logger.Error($"Error occured during running {methodName}", ex);
				throw;
			}
		}

		internal static async Task RunWithLoggingAsync(ILogger logger, Func<Task> func, string methodName)
		{
			try
			{
				logger.Info($"Start executing method {methodName}.");
				await func();
				logger.Info($"End executing method {methodName}.");
			}
			catch (Exception ex)
			{
				logger.Error($"Error occured during running {methodName}", ex);
				throw;
			}
		}
	}
}