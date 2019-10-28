using Potestas.Configuration;
using Potestas.Processors;

namespace Potestas.Factories
{
    public class FileProcessingFactory<T> : FileBaseProcessingFactory<T> where T : IEnergyObservation
    {
        public FileProcessingFactory(IConfiguration configuration) : base(configuration)
        {
        }

        public override IEnergyObservationProcessor<T> CreateProcessor()
        {
            return new SaveToFileProcessor<T>(new SerializeProcessor<T>(this.GetSerializer()), this.GetFilePath());
        }
    }
}