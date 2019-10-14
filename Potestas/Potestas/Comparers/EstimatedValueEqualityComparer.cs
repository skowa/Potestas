using Potestas.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Potestas.Comparers
{
    /// <summary>
    /// Class that makes equality comparison of <see cref="IEnergyObservation"/> instances by EstimatedValue.
    /// </summary>
    public class EstimatedValueEqualityComparer : IEqualityComparer<IEnergyObservation>
    {
        private readonly double _precision;

        /// <summary>
        /// Initializes a new instance of <see cref="EstimatedValueEqualityComparer"/>
        /// </summary>
        public EstimatedValueEqualityComparer()
        {
            if (!double.TryParse(ConfigurationManager.AppSettings["precision"], out _precision))
            {
                _precision = 0.000000001;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="IEnergyObservation"/> are equal by EstimatedValue.
        /// </summary>
        /// <param name="x">The first object of type <see cref="IEnergyObservation"/> to compare.</param>
        /// <param name="y">The second object of type <see cref="IEnergyObservation"/> to compare.</param>
        /// <returns><see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</returns>
        public bool Equals(IEnergyObservation x, IEnergyObservation y)
        {
            return object.ReferenceEquals(x, y) || x != null && y != null && x.EstimatedValue.CompareTo(y.EstimatedValue, _precision) == 0;
        }

        /// <summary>
        /// Returns a hash code for the specified <paramref name="obj"/> by EstimatedValue.
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

            return obj.EstimatedValue.GetHashCode();
        }
    }
}