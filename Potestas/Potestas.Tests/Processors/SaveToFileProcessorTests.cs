using System.Collections.Generic;
using System.IO;
using Potestas.Observations;
using Potestas.Processors;
using Xunit;

namespace Potestas.Tests.Processors
{
    public class SaveToFileProcessorTests : BaseProcessorTests
    {
        [Fact]
        public void OnNextTest_SomeFlashObservations_TheyAreWrittenCorrectlyToTheFile()
        {
            var filePath = "testFile.txt";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            List<FlashObservation> flashObservations = this.CreateFlashObservations();
            using var stream = new MemoryStream();
            var serializer = new BinarySerializer<FlashObservation>();
            serializer.Serialize(stream, flashObservations[0]);
            serializer.Serialize(stream, flashObservations[1]);
            serializer.Serialize(stream, flashObservations[2]);

            stream.Position = 0;
            using var readerOfExpectedContent = new StreamReader(stream);
            string expected = readerOfExpectedContent.ReadToEnd();
            readerOfExpectedContent.Close();

            using (var saveToFileProcessor = new SaveToFileProcessor<FlashObservation>(new SerializeProcessor<FlashObservation>(serializer), filePath))
            {
                saveToFileProcessor.OnNext(flashObservations[0]);
                saveToFileProcessor.OnNext(flashObservations[1]);
                saveToFileProcessor.OnNext(flashObservations[2]);
            }

            var actual = File.ReadAllText(filePath);

            Assert.Equal(expected, actual);
        }
    }
}