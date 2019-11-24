using System;
using Potestas.Analizers;
using Potestas.Configuration;
using Potestas.Observations;
using Potestas.OrmPlugin.Storages;
using Potestas.Processors;

namespace Potestas.OrmPlugin.Factories
{
    public class EFProcessingFactory : IProcessingFactory<FlashObservation>
    {
        private readonly IConfiguration _configuration;

        public EFProcessingFactory(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IEnergyObservationProcessor<FlashObservation> CreateProcessor() => new SaveToStorageProcessor<FlashObservation>(this.CreateStorage());

        public IEnergyObservationStorage<FlashObservation> CreateStorage() => new FlashObservationsSqlStorageEF(_configuration);

        public IEnergyObservationAnalizer CreateAnalizer() => new LINQAnalizer<FlashObservation>(this.CreateStorage());
    }
}