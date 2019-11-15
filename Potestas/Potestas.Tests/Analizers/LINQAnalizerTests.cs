using Potestas.Analizers;
using Potestas.Observations;

namespace Potestas.Tests.Analizers
{
	public class LINQAnalizerTests : BaseAnalyzerTests
	{
        protected override IEnergyObservationAnalizer GetAnalyzer()
        {
            return new LINQAnalizer<FlashObservation>(ListStorage);
        }
    }
}