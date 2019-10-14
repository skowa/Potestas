using System;
using Moq;
using Potestas.Comparers;
using Potestas.Tests.TestHelpers;
using Xunit;

namespace Potestas.Tests.Comparers
{
    public class EstimatedValueEqualityComparerTests
    {
        private readonly EstimatedValueEqualityComparer _estimatedValueEqualityComparer = new EstimatedValueEqualityComparer();
        private readonly Mock<IEnergyObservation> _firstObservationMock = new Mock<IEnergyObservation>();
        private readonly Mock<IEnergyObservation> _secondObservationMock = new Mock<IEnergyObservation>();

        [Theory]
        [ClassData(typeof(EqualsWithNullsTestData))]
        public void EqualsTest_FirstAndSecondIEnergyObservations_CorrectEqualityComparisonIsMade(IEnergyObservation first, IEnergyObservation second, bool expected)
        {
            bool actual = _estimatedValueEqualityComparer.Equals(first, second);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EqualsTest_FirstAndSecondIEnergyObservationsHaveTheSameEstimatedValue_FirstAndSecondAreEqual()
        {
            _firstObservationMock.Setup(o => o.EstimatedValue).Returns(2.2);
            _secondObservationMock.Setup(o => o.EstimatedValue).Returns(2.2);

            bool actual = _estimatedValueEqualityComparer.Equals(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.True(actual);
        }

        [Fact]
        public void EqualsTest_FirstAndSecondIEnergyObservationsHaveNotTheSameEstimatedValue_FirstAndSecondAreNotEqual()
        {
            _firstObservationMock.Setup(o => o.EstimatedValue).Returns(2);
            _secondObservationMock.Setup(o => o.EstimatedValue).Returns(2.2);

            bool actual = _estimatedValueEqualityComparer.Equals(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.False(actual);
        }

        [Fact]
        public void GetHashCodeTest_IEnergyObservation_EstimatedValueHashCodeIsReturned()
        {
            var estimatedValue = 2.2;
            _firstObservationMock.Setup(o => o.EstimatedValue).Returns(estimatedValue);
            int expected = estimatedValue.GetHashCode();

            int actual = _estimatedValueEqualityComparer.GetHashCode(_firstObservationMock.Object);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetHashCodeTest_IEnergyObservationIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _estimatedValueEqualityComparer.GetHashCode(null));
        }
    }
}