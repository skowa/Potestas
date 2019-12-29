using System;
using Potestas.Configuration;
using Potestas.Observations;
using Potestas.Processors;
using Potestas.WebPlugin.Analyzers;
using Potestas.WebPlugin.Storages;

namespace Potestas.WebPlugin.Factories
{
	public class WebProcessingFactory : IProcessingFactory<FlashObservation>
	{
		private readonly IConfiguration _configuration;
		
		public WebProcessingFactory(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public IEnergyObservationProcessor<FlashObservation> CreateProcessor() => new SaveToStorageProcessor<FlashObservation>(this.CreateStorage());

		public IEnergyObservationStorage<FlashObservation> CreateStorage() => new FlashObservationWebStorage(_configuration);

		public IEnergyObservationAnalizer CreateAnalizer() => new WebAnalyzer(_configuration);
	}
}