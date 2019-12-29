using System;
using System.Threading.Tasks;

namespace Potestas.WebPlugin.Services
{
	internal class ExponentialBackoff
	{
		private readonly int _maxRetries;
		private readonly int _delayMs;
		private readonly int _maxDelayMs;
		private int _retries;
		private int _pow;
		private readonly int _maxIntPow;
		
		internal ExponentialBackoff(int maxRetries, int delayMs, int maxDelayMs)
		{
			_maxRetries = maxRetries;
			_delayMs = delayMs;
			_maxDelayMs = maxDelayMs;
			_retries = 0;
			_pow = 1;
			_maxIntPow = 31;
		}

		public int Delay()
		{
			if (_retries == _maxRetries)
			{
				throw new TimeoutException("Max retry attempts exceeded.");
			}

			++_retries;
			if (_retries < _maxIntPow)
			{
				_pow *= 2;
			}

			int delay = Math.Min(_delayMs * (_pow - 1) / 2, _maxDelayMs);
			
			return delay;
		}

		public Task DelayAsync() => Task.Delay(this.Delay());
	}
}