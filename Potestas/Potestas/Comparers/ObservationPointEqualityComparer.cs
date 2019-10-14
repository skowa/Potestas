using System;
using System.Collections.Generic;

namespace Potestas.Comparers
{
    /// <summary>
    /// Class that makes equality comparison of <see cref="IEnergyObservation"/> instances by ObservationPoint.
    /// </summary>
    public class ObservationPointEqualityComparer : IEqualityComparer<IEnergyObservation>
    {
        /// <summary>
        /// Determines whether the specified <see cref="IEnergyObservation"/> are equal by ObservationPoint.
        /// </summary>
        /// <param name="x">The first object of type <see cref="IEnergyObservation"/> to compare.</param>
        /// <param name="y">The second object of type <see cref="IEnergyObservation"/> to compare.</param>
        /// <returns><see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</returns>
        public bool Equals(IEnergyObservation x, IEnergyObservation y)
        {
            return ReferenceEquals(x, y) || x != null && y != null && x.ObservationPoint.Equals(y.ObservationPoint);
        }

        /// <summary>
        /// Returns a hash code for the specified <paramref name="obj"/> by ObservationPoint.
        /// </summary>
        /// <param name="obj">The <see cref="IEnergyObservation" /> for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <exception cref="ArgumentNullException">The type of <paramref name="obj" /> is <see langword="null" />.</exception>
        public int GetHashCode(IEnergyObservation obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return obj.ObservationPoint.GetHashCode();
        }
    }
}