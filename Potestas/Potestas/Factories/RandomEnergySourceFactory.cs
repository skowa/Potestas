using Potestas.Logging;
using Potestas.Logging.Decorators;
using Potestas.Observations;
using Potestas.Sources;

namespace Potestas.Factories
{
    public class RandomEnergySourceFactory : ISourceFactory<FlashObservation>
    {
	    private readonly ILogger _logger;

	    public RandomEnergySourceFactory(ILogger logger)
	    {
		    _logger = logger;
	    }

        public IEnergyObservationSource<FlashObservation> CreateSource() => new LogEnergyObservationSourceDecorator<FlashObservation>(new RandomEnergySource(), _logger);

        public IEnergyObservationEventSource<FlashObservation> CreateEventSource() => new RandomEnergySource();
    }
}