using System.Collections.Generic;
using System.IO;
using Potestas.NoSqlPlugin.Processors;
using Potestas.Observations;
using Potestas.Processors;
using Potestas.Tests.TestHelpers;
using Xunit;

namespace Potestas.Tests.Processors
{
    public class SerializeToJsonProcessorTests
    {
        [Fact]
        public void OnNextTest_SomeFlashObservations_TheyAreWrittenCorrectlyToTheStream()
        {
            List<FlashObservation> flashObservations = FlashObservationBaseData.InitializeFlashObservations();

            var filePath = "data.json";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var processor = new SerializeToJsonProcessor<FlashObservation>();
            using var saveProcessor = new SaveToFileProcessor<FlashObservation>(processor, filePath);

            saveProcessor.OnNext(flashObservations[0]);
            saveProcessor.OnNext(flashObservations[1]);
            saveProcessor.OnNext(flashObservations[2]);
            saveProcessor.OnNext(flashObservations[3]);
            saveProcessor.OnCompleted();
        }
    }
}