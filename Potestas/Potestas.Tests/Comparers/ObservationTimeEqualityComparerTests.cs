using System;
using Moq;
using Potestas.Comparers;
using Potestas.Tests.TestHelpers;
using Xunit;

namespace Potestas.Tests.Comparers
{
    public class ObservationTimeEqualityComparerTests
    {
        private readonly ObservationTimeEqualityComparer _observationTimeEqualityComparer = new ObservationTimeEqualityComparer();
        private readonly Mock<IEnergyObservation> _firstObservationMock = new Mock<IEnergyObservation>();
        private readonly Mock<IEnergyObservation> _secondObservationMock = new Mock<IEnergyObservation>();

        [Theory]
        [ClassData(typeof(EqualsWithNullsTestData))]
        public void EqualsTest_FirstAndSecondIEnergyObservations_CorrectEqualityComparisonIsMade(IEnergyObservation first, IEnergyObservation second, bool expected)
        {
            bool actual = _observationTimeEqualityComparer.Equals(first, second);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EqualsTest_FirstAndSecondIEnergyObservationsHaveTheSameObservationTime_FirstAndSecondAreEqual()
        {
            _firstObservationMock.Setup(o => o.ObservationTime).Returns(new DateTime(2019, 10, 11));
            _secondObservationMock.Setup(o => o.ObservationTime).Returns(new DateTime(2019, 10, 11));

            bool actual = _observationTimeEqualityComparer.Equals(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.True(actual);
        }

        [Fact]
        public void EqualsTest_FirstAndSecondIEnergyObservationsHaveNotTheSameObservationTime_FirstAndSecondAreNotEqual()
        {
            _firstObservationMock.Setup(o => o.ObservationTime).Returns(new DateTime(2019, 10, 11));
            _secondObservationMock.Setup(o => o.ObservationTime).Returns(new DateTime(2019, 9, 11));

            bool actual = _observationTimeEqualityComparer.Equals(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.False(actual);
        }

        [Fact]
        public void GetHashCodeTest_IEnergyObservation_ObservationTimeHashCodeIsReturned()
        {
            var observationTime = new DateTime(2019, 10, 11);
            _firstObservationMock.Setup(o => o.ObservationTime).Returns(observationTime);
            int expected = observationTime.GetHashCode();

            int actual = _observationTimeEqualityComparer.GetHashCode(_firstObservationMock.Object);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetHashCodeTest_IEnergyObservationIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _observationTimeEqualityComparer.GetHashCode(null));
        }
    }
}