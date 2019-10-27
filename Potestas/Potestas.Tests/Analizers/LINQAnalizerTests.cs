using System;
using System.Collections.Generic;
using System.Linq;
using Potestas.Analizers;
using Potestas.Observations;
using Potestas.Storages;
using Xunit;

namespace Potestas.Tests.Analizers
{
	public class LINQAnalizerTests
	{
		private LINQAnalizer<FlashObservation> _linqAnalizer;
		private ListStorage<FlashObservation> _listStorage;

        public LINQAnalizerTests()
        {
            _listStorage = new ListStorage<FlashObservation>
            {
                new FlashObservation(new Coordinates(2, 5), 25, 100, new DateTime(2019, 10, 15)),
                new FlashObservation(new Coordinates(14, 0), 50, 200, new DateTime(2019, 10, 25)),
                new FlashObservation(new Coordinates(16, 5), 15, 300, new DateTime(2019, 10, 13)),
                new FlashObservation(new Coordinates(1, 16), 30, 400, new DateTime(2019, 10, 29)),
                new FlashObservation(new Coordinates(1, 16), 25, 100, new DateTime(2019, 10, 15)),
                new FlashObservation(new Coordinates(2, 5), 50, 200, new DateTime(2019, 10, 30)),
                new FlashObservation(new Coordinates(1, 16), 30, 150, new DateTime(2019, 10, 26)),
                new FlashObservation(new Coordinates(14, 0), 15, 300, new DateTime(2019, 10, 17)),
                new FlashObservation(new Coordinates(2, 5), 35, 400, new DateTime(2019, 10, 29))
            };

            _linqAnalizer = new LINQAnalizer<FlashObservation>(_listStorage);
        }

        [Fact]
        public void GetAverageEnergyTests_CorrectAverageValueIsReturned()
        {
            var expected = _listStorage.Average(o => o.EstimatedValue);

            var actual = _linqAnalizer.GetAverageEnergy();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAverageEnergyTests_DatesFromTo_CorrectAverageValueIsReturned()
        {
            double expected = 4800;

            var actual = _linqAnalizer.GetAverageEnergy(new DateTime(2019, 10, 15), new DateTime(2019, 10, 26));

            Assert.Equal(expected, actual);
        }

        [Fact]
		public void GetAverageEnergyTests_Coordinates_CorrectAverageValueIsReturned()
		{
			double expected = 7750;

			var actual = _linqAnalizer.GetAverageEnergy(new Coordinates(2, 10), new Coordinates(20, 5));

			Assert.Equal(expected, actual);
		}

        [Fact]
        public void GetDistributionByCoordinatesTest_CorrectDictionaryIsReturned()
        {
            var firstKey = new Coordinates(2, 5);
            var secondKey = new Coordinates(1, 16);
            var thirdKey = new Coordinates(14, 0);
            var forthKey = new Coordinates(16, 5);

            var expected = new Dictionary<Coordinates, int>
            {
                [firstKey] = 3,
                [secondKey] = 3,
                [thirdKey] = 2,
                [forthKey] = 1
            };

            var actual = _linqAnalizer.GetDistributionByCoordinates();

            Assert.Equal(expected.Count, actual.Count);
            Assert.Equal(expected[firstKey], actual[firstKey]);
            Assert.Equal(expected[secondKey], actual[secondKey]);
            Assert.Equal(expected[thirdKey], actual[thirdKey]);
            Assert.Equal(expected[forthKey], actual[forthKey]);
        }

        [Fact]
        public void GetDistributionByEnergyValueTest_CorrectDictionaryIsReturned()
        {
            double firstKey = 2500;
            double secondKey = 10000;
            double thirdKey = 4500;
            double forthKey = 12000;
            double fifthKey = 14000;

            var expected = new Dictionary<double, int>
            {
                [firstKey] = 2,
                [secondKey] = 2,
                [thirdKey] = 3,
                [forthKey] = 1,
                [fifthKey] = 1
            };

            var actual = _linqAnalizer.GetDistributionByEnergyValue();

            Assert.Equal(expected.Count, actual.Count);
            Assert.Equal(expected[firstKey], actual[firstKey]);
            Assert.Equal(expected[secondKey], actual[secondKey]);
            Assert.Equal(expected[thirdKey], actual[thirdKey]);
            Assert.Equal(expected[forthKey], actual[forthKey]);
            Assert.Equal(expected[fifthKey], actual[fifthKey]);
        }

        [Fact]
        public void GetDistributionByObservationTimeTest_CorrectDictionaryIsReturned()
        {
            var firstKey = new DateTime(2019, 10, 15);
            var secondKey = new DateTime(2019, 10, 25);
            var thirdKey = new DateTime(2019, 10, 13);
            var forthKey = new DateTime(2019, 10, 29);
            var fifthKey = new DateTime(2019, 10, 30);
            var sixthKey = new DateTime(2019, 10, 26);
            var seventhKey = new DateTime(2019, 10, 17);

            var expected = new Dictionary<DateTime, int>
            {
                [firstKey] = 2,
                [secondKey] = 1,
                [thirdKey] = 1,
                [forthKey] = 2,
                [fifthKey] = 1,
                [sixthKey] = 1,
                [seventhKey] = 1
            };

            var actual = _linqAnalizer.GetDistributionByObservationTime();

            Assert.Equal(expected.Count, actual.Count);
            Assert.Equal(expected[firstKey], actual[firstKey]);
            Assert.Equal(expected[secondKey], actual[secondKey]);
            Assert.Equal(expected[thirdKey], actual[thirdKey]);
            Assert.Equal(expected[forthKey], actual[forthKey]);
            Assert.Equal(expected[fifthKey], actual[fifthKey]);
            Assert.Equal(expected[sixthKey], actual[sixthKey]);
            Assert.Equal(expected[seventhKey], actual[seventhKey]);
        }

        [Fact]
        public void GetMaxEnergyTest_MaxEnergyIsReturned()
        {
            double expected = 14000;

            var actual = _linqAnalizer.GetMaxEnergy();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMaxEnergyTest_ObservationPoint_MaxEnergyIsReturned()
        {
            double expected = 12000;

            var actual = _linqAnalizer.GetMaxEnergy(new Coordinates(1, 16));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMaxEnergyTest_ObservationTime_MaxEnergyIsReturned()
        {
            double expected = 14000;

            var actual = _linqAnalizer.GetMaxEnergy(new DateTime(2019, 10, 29));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMaxEnergyPositionTest_MaxEnergyIsReturned()
        {
            var expected = new Coordinates(16, 5);

            var actual = _linqAnalizer.GetMaxEnergyPosition();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMaxEnergyTimeTest_MaxEnergyIsReturned()
        {
            var expected = new DateTime(2019, 10, 30);

            var actual = _linqAnalizer.GetMaxEnergyTime();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMinEnergyTest_MinEnergyIsReturned()
        {
            double expected = 2500;

            var actual = _linqAnalizer.GetMinEnergy();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMinEnergyTest_ObservationPoint_MinEnergyIsReturned()
        {
            double expected = 2500;

            var actual = _linqAnalizer.GetMinEnergy(new Coordinates(1, 16));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMinEnergyTest_ObservationTime_MinEnergyIsReturned()
        {
            double expected = 12000;

            var actual = _linqAnalizer.GetMinEnergy(new DateTime(2019, 10, 29));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMinEnergyPositionTest_MinEnergyIsReturned()
        {
            var expected = new Coordinates(1, 16);

            var actual = _linqAnalizer.GetMinEnergyPosition();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMinEnergyTimeTest_MinEnergyIsReturned()
        {
            var expected = new DateTime(2019, 10, 13);

            var actual = _linqAnalizer.GetMinEnergyTime();

            Assert.Equal(expected, actual);
        }
    }
}