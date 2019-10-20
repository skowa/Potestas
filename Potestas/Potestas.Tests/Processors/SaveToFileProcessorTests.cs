using System;
using System.IO;
using Potestas.Observations;
using Potestas.Processors;
using Xunit;

namespace Potestas.Tests.Processors
{
    public class SaveToFileProcessorTests
    {
        [Fact]
        public void OnNextTest_SomeFlashObservations_TheyAreWrittenCorrectlyToTheFile()
        {
            var filePath = "testFile.txt";
            File.WriteAllText(filePath, string.Empty);
            var serializer = new BinarySerializer<FlashObservation>();
            string expected;
            using (var saveToFileProcessor = new SaveToFileProcessor<FlashObservation>(new SerializeProcessor<FlashObservation>(serializer), filePath))
            {
                var flashObservation0 =
                    new FlashObservation(new Coordinates(2, 4), 23.4, 1000, new DateTime(2019, 10, 20));
                var flashObservation1 =
                    new FlashObservation(new Coordinates(3, 4), 23.4, 1000, new DateTime(2019, 10, 20));
                var flashObservation2 =
                    new FlashObservation(new Coordinates(5, 4), 23.4, 1000, new DateTime(2019, 10, 20));

                using var stream = new MemoryStream();
                serializer.Serialize(stream, flashObservation0);
                serializer.Serialize(stream, flashObservation1);
                serializer.Serialize(stream, flashObservation2);

                stream.Position = 0;
                using var readerOfExpectedContent = new StreamReader(stream);
                expected = readerOfExpectedContent.ReadToEnd();
                readerOfExpectedContent.Close();

                saveToFileProcessor.OnNext(flashObservation0);
                saveToFileProcessor.OnNext(flashObservation1);
                saveToFileProcessor.OnNext(flashObservation2);
            }

            var actual = File.ReadAllText(filePath);

            Assert.Equal(expected, actual);
        }
    }
}