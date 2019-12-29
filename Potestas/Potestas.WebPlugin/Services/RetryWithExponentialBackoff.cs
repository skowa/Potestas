using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Potestas.WebPlugin.Services
{
	internal class RetryWithExponentialBackoff
	{
		private readonly int _maxRetries;
		private readonly int _delayMs;
		private readonly int _maxDelayMs;

		internal RetryWithExponentialBackoff(int maxRetries = 50, int delayMs = 200, int maxDelayMs = 2000)
		{
			_maxRetries = maxRetries;
			_delayMs = delayMs;
			_maxDelayMs = maxDelayMs;
		}

		public TResponse Run<TResponse>(Func<TResponse> func)
		{
			var backoff = new ExponentialBackoff(_maxRetries, _delayMs, _maxDelayMs);
			while(true)
			{
				try
				{
					return func();
				}
				catch (Exception ex) when (ex is WebException ||
				                           ex is TimeoutException)
				{
					Thread.Sleep(backoff.Delay());
				}
			}
		}

		public async Task RunAsync(Func<Task> func)
		{
			var backoff = new ExponentialBackoff(_maxRetries, _delayMs, _maxDelayMs);
			while (true)
			{
				try
				{
					await func();
				}
				catch (Exception ex) when (ex is WebException ||
				                           ex is TimeoutException)
				{
					await backoff.DelayAsync();
				}
			}
		}
	}
}