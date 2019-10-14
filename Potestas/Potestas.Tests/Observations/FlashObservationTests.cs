using System;
using System.Collections.Generic;
using Potestas.Observations;
using Xunit;

namespace Potestas.Tests.Observations
{
    public class FlashObservationTests
    {
        public static List<object[]> EqualsData =>
            new List<object[]>
            {
                new object[] {new FlashObservation(new Coordinates(2, 3), 12.23, 10000, new DateTime(2019, 10, 20)), 
                    new FlashObservation(new Coordinates(2, 3), 12.23, 10000, new DateTime(2019, 10, 20)), true},
                new object[] {new FlashObservation(new Coordinates(2, 3), 12.23, 10000, new DateTime(2019, 10, 20)),
                    new FlashObservation(new Coordinates(1, 3), 12.23, 10000, new DateTime(2019, 10, 20)), false}
            };

        [Theory]
        [MemberData(nameof(EqualsData))]
        public void EqualityOpTest_TwoOperands_CorrectEqualityIsMade(FlashObservation first, FlashObservation second, bool expected)
        {
            var actual = first == second;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(EqualsData))]
        public void InequalityOpTest_TwoOperands_CorrectInequalityIsMade(FlashObservation first, FlashObservation second, bool unexpected)
        {
            var actual = first != second;

            Assert.Equal(!unexpected, actual);
        }

        [Theory]
        [MemberData(nameof(EqualsData))]
        public void EqualsTest_TwoOperands_CorrectEqualityIsMade(FlashObservation first, FlashObservation second, bool expected)
        {
            var actual = first.Equals(second);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(EqualsData))]
        public void EqualsVirtualTest_TwoOperands_CorrectEqualityIsMade(FlashObservation first, FlashObservation second, bool expected)
        {
            var actual = first.Equals((object)second);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EqualsVirtualTest_OneOperandIsNotFlashObservation_FalseIsReturned()
        {
            var first = new FlashObservation();
            var second = new object();
           
            var actual = first.Equals(second);

            Assert.False(actual);
        }

        [Fact]
        public void GetHashCodeTest_TwoDifferentCoordinates_TheirHashCodesDiffer()
        {
            var first = new FlashObservation(new Coordinates(2, 3), 220, 100, new DateTime(2019, 10, 20));
            var second = new FlashObservation(new Coordinates(2, 3), 220, 101, new DateTime(2019, 10, 20));

            var hashCodeFirst = first.GetHashCode();
            var hashCodeSecond = second.GetHashCode();

            Assert.True(hashCodeFirst != hashCodeSecond);
        }

        [Fact]
        public void ToStringTest()
        {
            var flashObservation = new FlashObservation(new Coordinates(2, 3), 220, 100, new DateTime(2019, 10, 20));

            var actual = flashObservation.ToString();

            Assert.Equal("FlashObservation: ObservationPoint = Coordinates: X = 2, Y = 3, Intensity = 220, Duration(ms) = 100, ObservationTime = 10/20/2019 12:00:00 AM, EstimatedValue = 22000", actual);
        }
    }
}