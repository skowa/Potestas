using Moq;
using Potestas.Comparers;
using Potestas.Configuration;
using Potestas.Tests.TestHelpers;
using Xunit;

namespace Potestas.Tests.Comparers
{
    public class EstimatedValueComparerTests
    {
        private readonly EstimatedValueComparer _estimatedValueComparer;
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
        private readonly Mock<IEnergyObservation> _firstObservationMock = new Mock<IEnergyObservation>();
        private readonly Mock<IEnergyObservation> _secondObservationMock = new Mock<IEnergyObservation>();

        public EstimatedValueComparerTests()
        {
            _estimatedValueComparer = new EstimatedValueComparer(_configurationMock.Object);
        }

        [Theory]
        [ClassData(typeof(CompareWithNullsTestData))]
        public void CompareTest_FirstAndSecondIEnergyObservations_CorrectComparisonIsMade(IEnergyObservation first, IEnergyObservation second, int expected)
        {
            int actual = _estimatedValueComparer.Compare(first, second);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CompareTest_IEnergyObservationFirstEstimatedValueIsBiggerThanEstimatedValueOfSecond_IEnergyObservationFirstIsBigger()
        {
            _firstObservationMock.Setup(o => o.EstimatedValue).Returns(2.2);
            _secondObservationMock.Setup(o => o.EstimatedValue).Returns(1.3);

            int actual = _estimatedValueComparer.Compare(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.Equal(1, actual);
        }

        [Fact]
        public void CompareTest_IEnergyObservationFirstEstimatedValueIsSmallerThanEstimatedValueOfSecond_IEnergyObservationFirstIsSmaller()
        {
            _firstObservationMock.Setup(o => o.EstimatedValue).Returns(2.2);
            _secondObservationMock.Setup(o => o.EstimatedValue).Returns(2.3);

            int actual = _estimatedValueComparer.Compare(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.Equal(-1, actual);
        }

        [Fact]
        public void CompareTest_IEnergyObservationFirstEstimatedValueIsTheSameAsEstimatedValueOfSecond_FirstAndSecondAreEqual()
        {
            _firstObservationMock.Setup(o => o.EstimatedValue).Returns(2.2);
            _secondObservationMock.Setup(o => o.EstimatedValue).Returns(2.2);

            int actual = _estimatedValueComparer.Compare(_firstObservationMock.Object, _secondObservationMock.Object);

            Assert.Equal(0, actual);
        }
    }
}