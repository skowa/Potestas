using Potestas.Configuration;
using Potestas.Observations;
using Potestas.SqlPlugin.Analyzers;
using Potestas.SqlPlugin.Processors;
using Potestas.SqlPlugin.Storages;

namespace Potestas.SqlPlugin.Factories
{
    public class SqlProcessingFactory : IProcessingFactory<FlashObservation>
    {
        private readonly IConfiguration _configuration;

        public SqlProcessingFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnergyObservationProcessor<FlashObservation> CreateProcessor() => new SaveFlashObservationToSqlProcessor(_configuration);

        public IEnergyObservationStorage<FlashObservation> CreateStorage() => new FlashObservationsSqlStorage(_configuration);

        public IEnergyObservationAnalizer CreateAnalizer() => new SqlAnalyzer(_configuration);
    }
}