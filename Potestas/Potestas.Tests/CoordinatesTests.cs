using System;
using System.Collections.Generic;
using Xunit;

namespace Potestas.Tests
{
    public class CoordinatesTests
    {
        public static List<object[]> EqualsData =>
            new List<object[]>
            {
                new object[] {new Coordinates(1.2343, 2.2345), new Coordinates(1.2345, 2.2344), true},
                new object[] {new Coordinates(1.2343, 2.2345), new Coordinates(1.1345, 2.3344), false}
            };

        [Fact]
        public void SetXTest_TheValueIsInRange_TheXIsSet()
        {
            var coordinates = new Coordinates();

            coordinates.X = 30;

            Assert.Equal(30, coordinates.X);
        }

        [Theory]
        [InlineData(-91)]
        [InlineData(91)]
        public void SetXTest_TheValueIsNotInRange_ArgumentOutOfRangeExceptionIsThrown(double value)
        {
            var coordinates = new Coordinates();

            Assert.Throws<ArgumentOutOfRangeException>(() => coordinates.X = value);
        }

        [Fact]
        public void SetYTest_TheValueIsInRange_TheYIsSet()
        {
            var coordinates = new Coordinates();

            coordinates.Y = 30;

            Assert.Equal(30, coordinates.Y);
        }

        [Theory]
        [InlineData(-0.1)]
        [InlineData(180.1)]
        public void SetYTest_TheValueIsNotInRange_ArgumentOutOfRangeExceptionIsThrown(double value)
        {
            var coordinates = new Coordinates();

            Assert.Throws<ArgumentOutOfRangeException>(() => coordinates.Y = value);
        }

        [Fact]
        public void AddOpTest_Operands_NewCoordinatesIsReturned()
        {
            var firstCoordinates = new Coordinates(10, 20);
            var secondCoordinates = new Coordinates(20, 30);

            var actualCoordinates = firstCoordinates + secondCoordinates;

            Assert.Equal(30, actualCoordinates.X);
            Assert.Equal(50, actualCoordinates.Y);
        }

        [Fact]
        public void AddOpTest_OperandsWhichSumIsInvalidCoordinates_ArgumentExceptionIsThrown()
        {
            var firstCoordinates = new Coordinates(80, 20);
            var secondCoordinates = new Coordinates(20, 30);

            Assert.Throws<InvalidOperationException>(() => firstCoordinates + secondCoordinates);
        }


        [Fact]
        public void SubtractOpTest_Operands_NewCoordinatesIsReturned()
        {
            var firstCoordinates = new Coordinates(10, 30);
            var secondCoordinates = new Coordinates(20, 20);

            var actualCoordinates = firstCoordinates - secondCoordinates;

            Assert.Equal(-10, actualCoordinates.X);
            Assert.Equal(10, actualCoordinates.Y);
        }

        [Fact]
        public void SubtractOpTest_OperandsWhichSubtractIsInvalidCoordinates_ArgumentExceptionIsThrown()
        {
            var firstCoordinates = new Coordinates(10, 20);
            var secondCoordinates = new Coordinates(20, 30);

            Assert.Throws<InvalidOperationException>(() => firstCoordinates - secondCoordinates);
        }

        [Theory]
        [MemberData(nameof(EqualsData))]
        public void EqualityOperatorTest_TwoOperands_CorrectEqualityIsMade(Coordinates first, Coordinates second, bool expected)
        {
            var actual = first == second;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(EqualsData))]
        public void InequalityOperatorTest_TwoOperands_CorrectInequalityIsMade(Coordinates first, Coordinates second, bool unexpected)
        {
            var actual = first != second;

            Assert.Equal(!unexpected, actual);
        }

        [Theory]
        [MemberData(nameof(EqualsData))]
        public void EqualsTest_TwoOperands_CorrectEqualityIsMade(Coordinates first, Coordinates second, bool expected)
        {
            var actual = first.Equals(second);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(EqualsData))]
        public void VirtualEqualsTest_TwoOperands_CorrectEqualityIsMade(Coordinates first, Coordinates second, bool expected)
        {
            var actual = first.Equals((object)second);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VirtualEqualsTest_OneOperandIsNotCoordinates_FalseIsReturned()
        {
            var first = new Coordinates();
            var second = new object();

            var actual = first.Equals(second);

            Assert.False(actual);
        }

        [Fact]
        public void GetHashCodeTest_TwoDifferentCoordinates_TheirHashCodesDiffer()
        {
            var first = new Coordinates(2, 3);
            var second = new Coordinates(3, 2);

            var hashCodeFirst = first.GetHashCode();
            var hashCodeSecond = second.GetHashCode();

            Assert.True(hashCodeFirst != hashCodeSecond);
        }

        [Fact]
        public void ToStringTest()
        {
            var coordinates = new Coordinates(20.3233, 10.3233);

            var actual = coordinates.ToString();

            Assert.Equal("Coordinates: X = 20.3233, Y = 10.3233", actual);
        }
    }
}