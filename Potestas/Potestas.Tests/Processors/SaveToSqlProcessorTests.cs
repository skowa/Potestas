using System.Collections.Generic;
using Moq;
using Potestas.Configuration;
using Potestas.Observations;
using Potestas.SqlPlugin.Processors;
using Potestas.Tests.TestHelpers;
using Xunit;

namespace Potestas.Tests.Processors
{
    public class SaveToSqlProcessorTests
    {
        //[Fact]
        public void OnNextTest_SomeFlashObservations_TheyAreWrittenCorrectlyToTheFile()
        {
            List<FlashObservation> flashObservations = FlashObservationBaseData.InitializeFlashObservations();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(m => m.GetValue("connectionString"))
                .Returns("Server=127.0.0.1,1423;Database=Potestas;User Id=SA;Password=DOCKERTASK_1");

            var sqlSave = new SaveFlashObservationToSqlProcessor(configurationMock.Object);
            sqlSave.OnNext(flashObservations[0]);
            sqlSave.OnNext(flashObservations[1]);
            sqlSave.OnNext(flashObservations[2]);
            sqlSave.OnCompleted();
        }
    }
}