using System;
using System.Collections;
using System.Collections.Generic;

namespace Potestas.Logging.Decorators
{
	public class LogEnergyObservationStorageDecorator<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
	{
		private readonly IEnergyObservationStorage<T> _innerStorage;
		private readonly ILogger _logger;

		public LogEnergyObservationStorageDecorator(IEnergyObservationStorage<T> innerStorage, ILogger logger)
		{
			_innerStorage = innerStorage ?? throw new ArgumentNullException(nameof(innerStorage));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public int Count => LoggerHelper.RunWithLogging(_logger, () => _innerStorage.Count, nameof(Count));
		public bool IsReadOnly => _innerStorage.IsReadOnly;
		public string Description => _innerStorage.Description;

		public IEnumerator<T> GetEnumerator() => LoggerHelper.RunWithLogging(_logger, () => _innerStorage.GetEnumerator(), nameof(GetEnumerator));

		public void Add(T item) => LoggerHelper.RunWithLogging(_logger, () => _innerStorage.Add(item), nameof(Add));

		public void Clear() => LoggerHelper.RunWithLogging(_logger, () => _innerStorage.Clear(), nameof(Clear));

		public bool Contains(T item) => LoggerHelper.RunWithLogging(_logger, () => _innerStorage.Contains(item), nameof(Contains));

		public void CopyTo(T[] array, int arrayIndex) => LoggerHelper.RunWithLogging(_logger, () => _innerStorage.CopyTo(array, arrayIndex), nameof(CopyTo));

		public bool Remove(T item) => LoggerHelper.RunWithLogging(_logger, () => _innerStorage.Remove(item), nameof(Remove));

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}