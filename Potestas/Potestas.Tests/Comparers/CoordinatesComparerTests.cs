using System.Collections.Generic;
using Moq;
using Potestas.Comparers;
using Potestas.Configuration;
using Xunit;

namespace Potestas.Tests.Comparers
{
    public class CoordinatesComparerTests
    {
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
        private readonly CoordinatesComparer _coordinatesComparer;

        public CoordinatesComparerTests()
        {
            _coordinatesComparer = new CoordinatesComparer(_configurationMock.Object);
        }

        public static List<object[]> CompareData =>
            new List<object[]>
            {
                new object[] {new Coordinates {X = 5.2, Y = 2.1}, new Coordinates {X = 3, Y = 2.3}, 1},
                new object[] {new Coordinates {X = 5.2, Y = 2.1}, new Coordinates {X = 8, Y = 2}, -1},
                new object[] {new Coordinates {X = 5.2, Y = 20}, new Coordinates {X = 5.2, Y = 2}, 1},
                new object[] {new Coordinates {X = 5.2, Y = 2}, new Coordinates {X = 5.2, Y = 20}, -1},
                new object[] {new Coordinates {X = 5.2, Y = 2.1}, new Coordinates {X = 5.2, Y = 2.1}, 0}
            };

        [Theory]
        [MemberData(nameof(CompareData))]
        public void CompareTest_FirstAndSecondCoordinates_CorrectComparisonIsMade(Coordinates first, Coordinates second, int expected)
        {
            int actual = _coordinatesComparer.Compare(first, second);

            Assert.Equal(expected, actual);
        }
    }
}