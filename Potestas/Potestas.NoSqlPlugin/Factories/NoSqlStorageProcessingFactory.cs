using Potestas.Analizers;
using Potestas.Configuration;
using Potestas.NoSqlPlugin.Storages;
using Potestas.Observations;
using Potestas.Processors;

namespace Potestas.NoSqlPlugin.Factories
{
	public class NoSqlStorageProcessingFactory: IProcessingFactory<FlashObservation>
	{
		private readonly IConfiguration _configuration;

		public NoSqlStorageProcessingFactory(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IEnergyObservationProcessor<FlashObservation> CreateProcessor() => new SaveToStorageProcessor<FlashObservation>(this.CreateStorage());

		public IEnergyObservationStorage<FlashObservation> CreateStorage() => new FlashObservationNoSqlStorage(_configuration);

		public IEnergyObservationAnalizer CreateAnalizer() => new LINQAnalizer<FlashObservation>(this.CreateStorage());
	}
}