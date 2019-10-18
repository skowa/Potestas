using Potestas.ExtensionMethods;
using System.Collections.Generic;
using Potestas.Configuration;

namespace Potestas.Comparers
{
    /// <summary>
    /// Class that compares <see cref="IEnergyObservation"/> instances by EstimatedValue.
    /// </summary>
    public class EstimatedValueComparer : BaseComparer<IEnergyObservation>, IComparer<IEnergyObservation>
    {
        private readonly double _precision;

        /// <summary>
        /// Initializes a new instance of <see cref="EstimatedValueComparer"/>
        /// </summary>
        public EstimatedValueComparer(IConfiguration configuration)
        {
            if (!double.TryParse(configuration.GetValue("precision"), out _precision))
            {
                _precision = 0.000000001;
            }
        }

        /// <summary>
        /// Compares two <see cref="IEnergyObservation"/> instances by EstimatedValue and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="IEnergyObservation"/> to compare.</param>
        /// <param name="y">The second <see cref="IEnergyObservation"/> to compare.</param>
        /// <returns>A signed integer that indicates the comparison result.</returns>
        public int Compare(IEnergyObservation x, IEnergyObservation y)
        {
            int? nullCompare = this.CompareConsideringNulls(x, y);

            return nullCompare ?? x.EstimatedValue.CompareTo(y.EstimatedValue, _precision);
        }
    }
}