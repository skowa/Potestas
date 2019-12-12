using Potestas.Configuration;
using Potestas.NoSqlPlugin.Analyzers;
using Potestas.NoSqlPlugin.Processors;
using Potestas.NoSqlPlugin.Storages;
using Potestas.Observations;

namespace Potestas.NoSqlPlugin.Factories
{
	public class NoSqlProcessingFactory : IProcessingFactory<FlashObservation>
	{
		private readonly IConfiguration _configuration;

		public NoSqlProcessingFactory(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IEnergyObservationProcessor<FlashObservation> CreateProcessor() => new SerializeToJsonProcessor<FlashObservation>();

		public IEnergyObservationStorage<FlashObservation> CreateStorage() => new FlashObservationNoSqlStorage(_configuration);

		public IEnergyObservationAnalizer CreateAnalizer() => new NoSqlAnalyzer(_configuration, "FlashObservations");
	}
}