using System;

namespace Potestas.ExtensionMethods
{
    internal static class DoubleExtensionMethods
    {
        internal static int CompareTo(this double x, double y, double precision)
        {
            double difference = x - y;

            return Math.Abs(difference) < precision ? 0 : difference > precision ? 1 : -1;
        }
    }
}