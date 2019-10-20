using System;
using System.IO;
using Moq;
using Potestas.Observations;
using Potestas.Processors;
using Xunit;

namespace Potestas.Tests.Processors
{
    public class SerializeProcessorTests
    { 
        private readonly SerializeProcessor<FlashObservation> _serializeProcessor;
        private readonly Mock<ISerializer<FlashObservation>> _serializerMock = new Mock<ISerializer<FlashObservation>>();
       
        public SerializeProcessorTests()
        {
            _serializeProcessor = new SerializeProcessor<FlashObservation>(_serializerMock.Object);
        }

        [Fact]
        public void OnNextTest_SomeFlashObservation_SerializeMethodIsCalled()
        {
            var flashObservation = new FlashObservation(new Coordinates(2, 4), 23.4, 1000, new DateTime(2019, 10, 20));
           
            _serializeProcessor.OnNext(flashObservation);

            _serializerMock.Verify(mock => mock.Serialize(It.IsAny<Stream>(), flashObservation), Times.Once);
        }
    }
}