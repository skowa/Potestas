using Potestas.Observations;
using Potestas.Sources;

namespace Potestas.Factories
{
    public class RandomEnergySourceFactory : ISourceFactory<FlashObservation>
    {
        public IEnergyObservationSource<FlashObservation> CreateSource() => new RandomEnergySource();

        public IEnergyObservationEventSource<FlashObservation> CreateEventSource() => new RandomEnergySource();
    }
}