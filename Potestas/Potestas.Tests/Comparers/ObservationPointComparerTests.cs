using System.Collections.Generic;
using Moq;
using Potestas.Comparers;
using Potestas.Tests.TestHelpers;
using Xunit;

namespace Potestas.Tests.Comparers
{
    public class ObservationPointComparerTests
    {
        private readonly ObservationPointComparer _observationPointComparer;
        private readonly Mock<IComparer<Coordinates>> _coordinatesComparerMock = new Mock<IComparer<Coordinates>>();
        private readonly Mock<IEnergyObservation> _firstObservationMock = new Mock<IEnergyObservation>();
        private readonly Mock<IEnergyObservation> _secondObservationMock = new Mock<IEnergyObservation>();

        public ObservationPointComparerTests()
        {
            _observationPointComparer = new ObservationPointComparer(_coordinatesComparerMock.Object);
        }

        [Theory]
        [ClassData(typeof(CompareWithNullsTestData))]
        public void CompareTest_FirstAndSecondIEnergyObservations_CorrectComparisonIsMade(IEnergyObservation first, IEnergyObservation second, int expected)
        {
            int actual = _observationPointComparer.Compare(first, second);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CompareTest_IEnergyObservationFirstObservationPointIsBiggerThanObservationPointOfSecond_IEnergyObservationFirstIsBigger()
        {
            _coordinatesComparerMock.Setup(c => c.Compare(It.IsAny<Coordinates>(), It.IsAny<Coordinates>())).Returns(1);

            int actual = _observationPointComparer.Compare(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.Equal(1, actual);
        }

        [Fact]
        public void CompareTest_IEnergyObservationFirstObservationPointIsSmallerThanObservationPointOfSecond_IEnergyObservationFirstIsSmaller()
        {
            _coordinatesComparerMock.Setup(c => c.Compare(It.IsAny<Coordinates>(), It.IsAny<Coordinates>())).Returns(-1);

            int actual = _observationPointComparer.Compare(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.Equal(-1, actual);
        }

        [Fact]
        public void CompareTest_IEnergyObservationFirstObservationPointIsTheSameAsObservationPointOfSecond_FirstAndSecondAreEqual()
        {
            _coordinatesComparerMock.Setup(c => c.Compare(It.IsAny<Coordinates>(), It.IsAny<Coordinates>())).Returns(0);

            int actual = _observationPointComparer.Compare(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.Equal(0, actual);
        }
    }
}