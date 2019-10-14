using System;
using System.Collections.Generic;

namespace Potestas.Comparers
{
    /// <summary>
    /// Class that compares <see cref="IEnergyObservation"/> instances by ObservationTime.
    /// </summary>
    public class ObservationTimeComparer : BaseComparer<IEnergyObservation>, IComparer<IEnergyObservation>
    {
        /// <summary>
        /// Compares two <see cref="IEnergyObservation"/> instances by ObservationTime and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="IEnergyObservation"/> to compare.</param>
        /// <param name="y">The second <see cref="IEnergyObservation"/> to compare.</param>
        /// <returns>A signed integer that indicates the comparison result.</returns>
        public int Compare(IEnergyObservation x, IEnergyObservation y)
        {
            int? nullCompare = this.CompareConsideringNulls(x, y);
            
            return nullCompare ?? DateTime.Compare(x.ObservationTime, y.ObservationTime);
        }
    }
}