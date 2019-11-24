using System;
using Potestas.Configuration;
using Potestas.Observations;
using Potestas.OrmPlugin.Analyzers;
using Potestas.OrmPlugin.DapperConfiguration;
using Potestas.OrmPlugin.Processors;
using Potestas.OrmPlugin.Storages;

namespace Potestas.OrmPlugin.Factories
{
    public class DapperProcessingFactory : IProcessingFactory<FlashObservation>
    {
        private readonly IConfiguration _configuration;

        public DapperProcessingFactory(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            DapperInitializer.InitDapper();
        }

        public IEnergyObservationProcessor<FlashObservation> CreateProcessor() => new SaveFlashObservationToSqlProcessor(_configuration);

        public IEnergyObservationStorage<FlashObservation> CreateStorage() => new FlashObservationsSqlStorage(_configuration);

        public IEnergyObservationAnalizer CreateAnalizer() => new SqlAnalyzer(_configuration);
    }
}