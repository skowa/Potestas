using System;
using System.Collections.Generic;
using Potestas.Observations;

namespace Potestas.Tests.Processors
{
    public abstract class BaseProcessorTests
    {
        protected List<FlashObservation> CreateFlashObservations()
        {
            return new List<FlashObservation>
            {
                new FlashObservation(new Coordinates(2, 4), 23.4, 1000, new DateTime(2019, 10, 20)),
                new FlashObservation(new Coordinates(3, 4), 23.4, 1000, new DateTime(2019, 10, 20)),
                new FlashObservation(new Coordinates(5, 4), 23.4, 1000, new DateTime(2019, 10, 20))
            };
        }
    }
}