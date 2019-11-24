using Potestas.Configuration;
using Potestas.Observations;
using Potestas.OrmPlugin.DapperConfiguration;
using Potestas.OrmPlugin.Processors;
using Xunit;

namespace Potestas.Tests.Processors
{
    public class SaveToSqlProcessorViaDapperTests : BaseSaveToSqlProcessorTests
    {
        public SaveToSqlProcessorViaDapperTests()
        {
            DapperInitializer.InitDapper();
        }

       // [Fact]
        public void OnNextTest()
        {
            this.OnNextTestInit();
        }

        public override IEnergyObservationProcessor<FlashObservation> CreateObservationProcessor(IConfiguration configuration)
        {
            return new SaveFlashObservationToSqlProcessor(configuration);
        }
    }
}