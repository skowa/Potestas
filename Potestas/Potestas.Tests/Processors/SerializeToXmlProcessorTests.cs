using System.Collections.Generic;
using System.IO;
using System.Xml;
using Potestas.Observations;
using Potestas.XmlPlugin;
using Xunit;

namespace Potestas.Tests.Processors
{
    public class SerializeToXmlProcessorTests : BaseProcessorTests
    {
        [Fact]
        public void OnNextTest_SomeFlashObservations_TheyAreWrittenCorrectlyToTheStream()
        {
            var xmlSerializer = new XmlSerializer<FlashObservation>();
            List<FlashObservation> flashObservations = this.CreateFlashObservations();

            using var serializeToXmlProcessor = new SerializeToXmlProcessor<FlashObservation>();
            serializeToXmlProcessor.OnNext(flashObservations[0]);
            serializeToXmlProcessor.OnNext(flashObservations[1]);
            serializeToXmlProcessor.OnNext(flashObservations[2]);
            serializeToXmlProcessor.OnCompleted();

            serializeToXmlProcessor.Stream.Seek(0, SeekOrigin.Begin);
            var xmlSettings = new XmlReaderSettings
            {
                IgnoreWhitespace = true
            };
            using var xmlReader = XmlReader.Create(serializeToXmlProcessor.Stream, xmlSettings);
            xmlReader.ReadStartElement();
            FlashObservation actual0 = xmlSerializer.Deserialize(xmlReader);
            FlashObservation actual1 = xmlSerializer.Deserialize(xmlReader);
            FlashObservation actual2 = xmlSerializer.Deserialize(xmlReader);
            xmlReader.ReadEndElement();

            Assert.Equal(flashObservations[0], actual0);
            Assert.Equal(flashObservations[1], actual1);
            Assert.Equal(flashObservations[2], actual2);
        }
    }
}