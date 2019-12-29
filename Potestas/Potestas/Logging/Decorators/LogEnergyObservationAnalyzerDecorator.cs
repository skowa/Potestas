using System;
using System.Collections.Generic;

namespace Potestas.Logging.Decorators
{
	public class LogEnergyObservationAnalyzerDecorator : IEnergyObservationAnalizer
	{
		private readonly IEnergyObservationAnalizer _innerAnalyzer;
		private readonly ILogger _logger;

		public LogEnergyObservationAnalyzerDecorator(IEnergyObservationAnalizer innerAnalyzer, ILogger logger)
		{
			_innerAnalyzer = innerAnalyzer ?? throw new ArgumentNullException(nameof(innerAnalyzer));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public IDictionary<double, int> GetDistributionByEnergyValue() => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetDistributionByEnergyValue(), nameof(GetDistributionByEnergyValue));

		public IDictionary<Coordinates, int> GetDistributionByCoordinates() => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetDistributionByCoordinates(), nameof(GetDistributionByCoordinates));

		public IDictionary<DateTime, int> GetDistributionByObservationTime() => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetDistributionByObservationTime(), nameof(GetDistributionByObservationTime));

		public double GetMaxEnergy() => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetMaxEnergy(), nameof(GetMaxEnergy));

		public double GetMaxEnergy(Coordinates coordinates) => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetMaxEnergy(coordinates), nameof(GetMaxEnergy));

		public double GetMaxEnergy(DateTime dateTime) => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetMaxEnergy(dateTime), nameof(GetMaxEnergy));

		public double GetMinEnergy() => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetMinEnergy(), nameof(GetMinEnergy));

		public double GetMinEnergy(Coordinates coordinates) => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetMinEnergy(coordinates), nameof(GetMinEnergy));

		public double GetMinEnergy(DateTime dateTime) => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetMinEnergy(), nameof(GetMinEnergy));

		public double GetAverageEnergy() => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetAverageEnergy(), nameof(GetAverageEnergy));

		public double GetAverageEnergy(DateTime startFrom, DateTime endBy) => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetAverageEnergy(startFrom, endBy), nameof(GetAverageEnergy));

		public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight) => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetAverageEnergy(rectTopLeft, rectBottomRight), nameof(GetAverageEnergy));

		public DateTime GetMaxEnergyTime() => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetMaxEnergyTime(), nameof(GetMaxEnergyTime));

		public Coordinates GetMaxEnergyPosition() => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetMaxEnergyPosition(), nameof(GetMaxEnergyPosition));

		public DateTime GetMinEnergyTime() => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetMinEnergyTime(), nameof(GetMinEnergyTime));

		public Coordinates GetMinEnergyPosition() => LoggerHelper.RunWithLogging(_logger,
			() => _innerAnalyzer.GetMinEnergyPosition(), nameof(GetMinEnergyPosition));
	}
}