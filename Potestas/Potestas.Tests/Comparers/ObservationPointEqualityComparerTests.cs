using System;
using Moq;
using Potestas.Comparers;
using Potestas.Tests.TestHelpers;
using Xunit;

namespace Potestas.Tests.Comparers
{
    public class ObservationPointEqualityComparerTests
    {
        private readonly ObservationPointEqualityComparer _observationPointEqualityComparer = new ObservationPointEqualityComparer();
        private readonly Mock<IEnergyObservation> _firstObservationMock = new Mock<IEnergyObservation>();
        private readonly Mock<IEnergyObservation> _secondObservationMock = new Mock<IEnergyObservation>();

        [Theory]
        [ClassData(typeof(EqualsWithNullsTestData))]
        public void EqualsTest_FirstAndSecondIEnergyObservations_CorrectEqualityComparisonIsMade(IEnergyObservation first, IEnergyObservation second, bool expected)
        {
            bool actual = _observationPointEqualityComparer.Equals(first, second);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EqualsTest_FirstAndSecondIEnergyObservationsHaveTheSameObservationPoint_FirstAndSecondAreEqual()
        {
            _firstObservationMock.Setup(o => o.ObservationPoint).Returns(new Coordinates{X = 2, Y = 3});
            _secondObservationMock.Setup(o => o.ObservationPoint).Returns(new Coordinates {X = 2, Y = 3});

            bool actual = _observationPointEqualityComparer.Equals(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.True(actual);
        }

        [Fact]
        public void EqualsTest_FirstAndSecondIEnergyObservationsHaveNotTheSameObservationPoint_FirstAndSecondAreNotEqual()
        {
            _firstObservationMock.Setup(o => o.ObservationPoint).Returns(new Coordinates {X = 2, Y = 3});
            _secondObservationMock.Setup(o => o.ObservationPoint).Returns(new Coordinates {X = 2, Y = 2});

            bool actual = _observationPointEqualityComparer.Equals(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.False(actual);
        }

        [Fact]
        public void GetHashCodeTest_IEnergyObservation_ObservationPointHashCodeIsReturned()
        {
            var observationPoint = new Coordinates {X = 2, Y = 3};
            _firstObservationMock.Setup(o => o.ObservationPoint).Returns(observationPoint);
            int expected = observationPoint.GetHashCode();

            int actual = _observationPointEqualityComparer.GetHashCode(_firstObservationMock.Object);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetHashCodeTest_IEnergyObservationIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _observationPointEqualityComparer.GetHashCode(null));
        }
    }
}