using System.Collections.Generic;
using System.Configuration;
using Potestas.ExtensionMethods;

namespace Potestas.Comparers
{
    /// <summary>
    /// Class that compares <see cref="Coordinates"/>.
    /// </summary>
    public class CoordinatesComparer : IComparer<Coordinates>
    {
        private readonly double _precision;

        /// <summary>
        /// Initializes a new instance of <see cref="CoordinatesComparer"/>
        /// </summary>
        public CoordinatesComparer()
        {
            if (!double.TryParse(ConfigurationManager.AppSettings["precision"], out _precision))
            {
                _precision = 0.000000001;
            }
        }


        /// <summary>
        /// Compares two <see cref="Coordinates"/> and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="Coordinates"/> to compare.</param>
        /// <param name="y">The second <see cref="Coordinates"/> to compare.</param>
        /// <returns>A signed integer that indicates the comparison result.</returns>
        public int Compare(Coordinates x, Coordinates y)
        {
            int xCoordinatesComparison = x.X.CompareTo(y.X, _precision);

            return xCoordinatesComparison != 0 ? xCoordinatesComparison : x.Y.CompareTo(y.Y, _precision);
        }
    }
}