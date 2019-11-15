using System;
using System.IO;
using Potestas.Observations;
using Potestas.Processors;
using Potestas.Tests.TestHelpers;
using Potestas.XmlPlugin;
using Xunit;

namespace Potestas.Tests.Storages
{
    public class XmlFileStorageTests
    {
        private readonly string _filePath = "xmlFileStorageTest.xml";
        private readonly XmlFileStorage<FlashObservation> _xmlFileStorage;

        public XmlFileStorageTests()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            _xmlFileStorage = new XmlFileStorage<FlashObservation>(_filePath);

            using var fileProcessor = new SaveToFileProcessor<FlashObservation>(new SerializeToXmlProcessor<FlashObservation>(), _filePath);
            var data = FlashObservationBaseData.InitializeFlashObservations();
            fileProcessor.OnNext(data[0]);
            fileProcessor.OnNext(data[1]);
            fileProcessor.OnNext(data[2]);
            fileProcessor.OnNext(data[3]);
            fileProcessor.OnCompleted();
        }

        [Fact]
        public void CountTest()
        {
            var expected = 4;

            var actual = _xmlFileStorage.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(14, 0, 50, 200, true)]
        [InlineData(3, 4, 15, 100, false)]
        public void ContainsTest_FlashObservationConstructorArguments(double x, double y, double intensity, int durationMs, bool expected)
        {
            var flashObservation = new FlashObservation(new Coordinates(x, y), intensity, durationMs, new DateTime(2019, 10, 25));

            bool actual = _xmlFileStorage.Contains(flashObservation);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(14, 0, 50, 200, true)]
        [InlineData(3, 4, 15, 100, false)]
        public void RemoveTest_FlashObservationConstructorArguments(double x, double y, double intensity, int durationMs, bool expected)
        {
            var flashObservation = new FlashObservation(new Coordinates(x, y), intensity, durationMs, new DateTime(2019, 10, 25));

            bool actual = _xmlFileStorage.Remove(flashObservation);

            Assert.Equal(expected, actual);
        }
    }
}