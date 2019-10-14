using System;
using Moq;
using Potestas.Comparers;
using Potestas.Tests.TestHelpers;
using Xunit;

namespace Potestas.Tests.Comparers
{
    public class ObservationTimeComparerTests
    {
        private readonly ObservationTimeComparer _observationTimeComparer = new ObservationTimeComparer();
        private readonly Mock<IEnergyObservation> _firstObservationMock = new Mock<IEnergyObservation>();
        private readonly Mock<IEnergyObservation> _secondObservationMock = new Mock<IEnergyObservation>();

        [Theory]
        [ClassData(typeof(CompareWithNullsTestData))]
        public void CompareTest_FirstAndSecondIEnergyObservations_CorrectComparisonIsMade(IEnergyObservation first, IEnergyObservation second, int expected)
        {
            int actual = _observationTimeComparer.Compare(first, second);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CompareTest_IEnergyObservationFirstObservationTimeIsBiggerThanObservationTimeOfSecond_IEnergyObservationFirstIsBigger()
        {
            _firstObservationMock.Setup(o => o.ObservationTime).Returns(new DateTime(2019, 10, 11));
            _secondObservationMock.Setup(o => o.ObservationTime).Returns(new DateTime(2019, 10, 10));

            int actual = _observationTimeComparer.Compare(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.Equal(1, actual);
        }

        [Fact]
        public void CompareTest_IEnergyObservationFirstObservationTimeIsSmallerThanObservationTimeOfSecond_IEnergyObservationFirstIsSmaller()
        {
            _firstObservationMock.Setup(o => o.ObservationTime).Returns(new DateTime(2019, 10, 10));
            _secondObservationMock.Setup(o => o.ObservationTime).Returns(new DateTime(2019, 10, 11));

            int actual = _observationTimeComparer.Compare(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.Equal(-1, actual);
        }

        [Fact]
        public void CompareTest_IEnergyObservationFirstObservationTimeIsTheSameAsObservationTimeOfSecond_FirstAndSecondAreEqual()
        {
            _firstObservationMock.Setup(o => o.ObservationTime).Returns(new DateTime(2019, 10, 11));
            _secondObservationMock.Setup(o => o.ObservationTime).Returns(new DateTime(2019, 10, 11));

            int actual = _observationTimeComparer.Compare(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.Equal(0, actual);
        }
    }
}