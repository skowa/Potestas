using System;
using System.Collections.Generic;
using Potestas.Observations;

namespace Potestas.Tests.TestHelpers
{
    public static class FlashObservationBaseData
    {
        public static List<FlashObservation> InitializeFlashObservations()
        {
            return new List<FlashObservation>
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
        }
    }
}