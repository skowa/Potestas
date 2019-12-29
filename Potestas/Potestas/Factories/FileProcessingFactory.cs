using Potestas.Configuration;
using Potestas.Logging;
using Potestas.Logging.Decorators;
using Potestas.Processors;

namespace Potestas.Factories
{
    public class FileProcessingFactory<T> : FileBaseProcessingFactory<T> where T : IEnergyObservation
    {
        public FileProcessingFactory(IConfiguration configuration, ILogger logger) : base(logger, configuration)
        {
        }

        public override IEnergyObservationProcessor<T> CreateProcessor()
        {
            return new LogEnergyObservationProcessorDecorator<T>(new SaveToFileProcessor<T>(new SerializeProcessor<T>(this.GetSerializer()), this.GetFilePath()), this.Logger);
        }
    }
}