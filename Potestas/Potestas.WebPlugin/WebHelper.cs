using System;
using Potestas.WebPlugin.Services;

namespace Potestas.WebPlugin
{
	internal static class WebHelper
	{
		internal static TResponse DecorateWithRetry<TResponse>(Func<TResponse> apiCall)
		{
			var retry = new RetryWithExponentialBackoff();
			return retry.Run(apiCall);
		}
	}
}