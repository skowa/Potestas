using Potestas.Analizers;
using Potestas.Processors;
using Potestas.Storages;

namespace Potestas.Factories
{
    [ExcludeFactoryCreation]
    public class ListStorageProcessingFactory<T> : IProcessingFactory<T> where T : IEnergyObservation
    {
        public IEnergyObservationProcessor<T> CreateProcessor() => new SaveToStorageProcessor<T>(this.CreateStorage());

        public IEnergyObservationStorage<T> CreateStorage() => new ListStorage<T>();

        public IEnergyObservationAnalizer CreateAnalizer() => new LINQAnalizer<T>(this.CreateStorage());
    }
}