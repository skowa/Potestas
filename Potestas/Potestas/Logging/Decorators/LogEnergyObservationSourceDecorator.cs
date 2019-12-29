using System;
using System.Threading;
using System.Threading.Tasks;

namespace Potestas.Logging.Decorators
{
	public class LogEnergyObservationSourceDecorator<T> : IEnergyObservationSource<T> where T : IEnergyObservation
	{
		private readonly IEnergyObservationSource<T> _innerSource;
		private readonly ILogger _logger;

		public LogEnergyObservationSourceDecorator(IEnergyObservationSource<T> innerSource, ILogger logger)
		{
			_innerSource = innerSource ?? throw new ArgumentNullException(nameof(innerSource));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public IDisposable Subscribe(IObserver<T> observer) =>
			LoggerHelper.RunWithLogging(_logger, () => _innerSource.Subscribe(observer), nameof(Subscribe));

		public string Description => _innerSource.Description;

		public Task Run(CancellationToken cancellationToken) =>
			LoggerHelper.RunWithLoggingAsync(_logger, () => _innerSource.Run(cancellationToken), nameof(Run));
	}
}