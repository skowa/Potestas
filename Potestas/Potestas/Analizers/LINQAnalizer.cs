using System;
using System.Collections.Generic;
using System.Linq;
using Potestas.Comparers;

namespace Potestas.Analizers
{
    /* TASK. Implement an Analizer for Observations using LINQ
     */
    public class LINQAnalizer<T> : IEnergyObservationAnalizer where T : IEnergyObservation
    {
        private readonly IEnergyObservationStorage<T> _energyObservationStorage;

        public LINQAnalizer(IEnergyObservationStorage<T> energyObservationStorage)
        {
            _energyObservationStorage = energyObservationStorage ?? throw new ArgumentNullException(nameof(energyObservationStorage));
        }

        public double GetAverageEnergy()
        {
            return _energyObservationStorage.Average(e => e.EstimatedValue);
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            return _energyObservationStorage.Where(e => e.ObservationTime >= startFrom && e.ObservationTime <= endBy)
                .Average(e => e.EstimatedValue);
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            return _energyObservationStorage.Where(e =>
                    e.ObservationPoint.X >= rectTopLeft.X && 
                    e.ObservationPoint.X <= rectBottomRight.X &&
                    e.ObservationPoint.Y <= rectTopLeft.Y && 
                    e.ObservationPoint.Y >= rectBottomRight.Y)
                .Average(e => e.EstimatedValue);
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            return _energyObservationStorage.GroupBy(e => e, new ObservationPointEqualityComparer())
                .ToDictionary(group => group.Key.ObservationPoint, group => group.Count());
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            return _energyObservationStorage.GroupBy(e => e, new EstimatedValueEqualityComparer(new Configuration.Configuration()))
                .ToDictionary(group => group.Key.EstimatedValue, group => group.Count());
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            return _energyObservationStorage.GroupBy(e => e, new ObservationTimeEqualityComparer())
                .ToDictionary(group => group.Key.ObservationTime, group => group.Count());
        }

        public double GetMaxEnergy()
        {
            return _energyObservationStorage.Max(e => e.EstimatedValue);
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            return _energyObservationStorage.Where(e => e.ObservationPoint == coordinates).Max(e => e.EstimatedValue);
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            return _energyObservationStorage.Where(e => e.ObservationTime == dateTime).Max(e => e.EstimatedValue);
        }

        public Coordinates GetMaxEnergyPosition()
        {
            // I don't think that Coordinates should implement IComparable, so using LINQ and IComparer<Coordinates> instance it is the only way.
            // Without constraint to use LINQ I would've implement it another way.
            return _energyObservationStorage.OrderBy(o => o.ObservationPoint, new CoordinatesComparer(new Configuration.Configuration())).Select(o => o.ObservationPoint).Last();
        }

        public DateTime GetMaxEnergyTime()
        {
            return _energyObservationStorage.Max(e => e.ObservationTime);
        }

        public double GetMinEnergy()
        {
            return _energyObservationStorage.Min(e => e.EstimatedValue);
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            return _energyObservationStorage.Where(e => e.ObservationPoint == coordinates).Min(e => e.EstimatedValue);
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            return _energyObservationStorage.Where(e => e.ObservationTime == dateTime).Min(e => e.EstimatedValue);
        }

        public Coordinates GetMinEnergyPosition()
        {
            return _energyObservationStorage.OrderBy(o => o.ObservationPoint, new CoordinatesComparer(new Configuration.Configuration())).Select(o => o.ObservationPoint).First();
        }

        public DateTime GetMinEnergyTime()
        {
            return _energyObservationStorage.Min(e => e.ObservationTime);
        }
    }
}
