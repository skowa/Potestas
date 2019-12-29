using System;

namespace Potestas.Logging.Decorators
{
	public class LogEnergyObservationProcessorDecorator<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
	{
		private readonly IEnergyObservationProcessor<T> _innerProcessor;
		private readonly ILogger _logger;

		public LogEnergyObservationProcessorDecorator(IEnergyObservationProcessor<T> processor, ILogger logger)
		{
			_innerProcessor = processor ?? throw new ArgumentNullException(nameof(processor));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public string Description => _innerProcessor.Description;

		public void OnCompleted() => LoggerHelper.RunWithLogging(_logger, () => _innerProcessor.OnCompleted(), nameof(OnCompleted));

		public void OnError(Exception error) => LoggerHelper.RunWithLogging(_logger, () => _innerProcessor.OnError(error), nameof(OnError));

		public void OnNext(T value) => LoggerHelper.RunWithLogging(_logger, () => _innerProcessor.OnNext(value), nameof(OnNext));

	}
}