using System;
using System.Collections.Generic;

namespace Potestas.Comparers
{
    /// <summary>
    /// Class that compares <see cref="IEnergyObservation"/> instances by ObservationPoint.
    /// </summary>
    public class ObservationPointComparer : BaseComparer<IEnergyObservation>, IComparer<IEnergyObservation>
    {
        private readonly Comparison<Coordinates> _compare;

        /// <summary>
        /// Initializes a new instance of <see cref="ObservationPointComparer"/>
        /// </summary>
        /// <param name="comparer">A comparer for <see cref="Coordinates"/></param>
        public ObservationPointComparer(IComparer<Coordinates> comparer) : this(comparer.Compare) { }

        /// <summary>
        /// Initializes a new instance of <see cref="ObservationPointComparer"/>
        /// </summary>
        /// <param name="compare">A compare for <see cref="Coordinates"/>.</param>
        public ObservationPointComparer(Comparison<Coordinates> compare)
        {
            _compare = compare;
        }

        /// <summary>
        /// Compares two <see cref="IEnergyObservation"/> instances by ObservationPoint and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="IEnergyObservation"/> to compare.</param>
        /// <param name="y">The second <see cref="IEnergyObservation"/> to compare.</param>
        /// <returns>A signed integer that indicates the comparison result.</returns>
        public int Compare(IEnergyObservation x, IEnergyObservation y)
        {
            int? nullCompare = this.CompareConsideringNulls(x, y);
            
            return nullCompare ?? _compare(x.ObservationPoint, y.ObservationPoint);
        }
    }
}