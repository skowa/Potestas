using Potestas.Configuration;
using Potestas.Processors;

namespace Potestas.Factories
{
    public class FileStorageProcessingFactory<T> : FileBaseProcessingFactory<T> where T : IEnergyObservation
    {
        public FileStorageProcessingFactory(IConfiguration configuration) : base(configuration)
        {
        }

        public override IEnergyObservationProcessor<T> CreateProcessor() => new SaveToStorageProcessor<T>(this.CreateStorage());
    }
}