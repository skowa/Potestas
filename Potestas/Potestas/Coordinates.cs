using Potestas.ExtensionMethods;
using System;

namespace Potestas
{
    /* TASK. Implement this structure: 
     * 1. Implement custom constructor
     * 2. The valid range for X is [-90; 90], for Y [0; 180]
     * 3. Take into account boxing and unboxing issues
     * 4. Implement + and - operators for this structure.
     * 5. Implement a way to represent coordinates in string.
     * 6. Coordinates are equal each other when each dimension values are equal with thousand precision
     * 7. Implement == and != operators for this structure.
     * 8. 
     */
    public struct Coordinates : IEquatable<Coordinates>
    {
        private const double Precision = 0.001;
        private const double MinXCoordinate = -90;
        private const double MaxXCoordinate = 90;
        private const double MinYCoordinate = 0;
        private const double MaxYCoordinate = 180;

        private double _x;
        private double _y;

        /// <summary>
        /// Initializes a new instance of <see cref="Coordinates"/>.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, when X or Y are not in range.</exception>
        public Coordinates(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Get or sets the X coordinate.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, when X is not in range.</exception>
        public double X
        {
            get => _x;
            set => SetCore(value, MinXCoordinate, MaxXCoordinate, ref _x);
        }

        /// <summary>
        /// Get or sets the Y coordinate.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, when Y is not in range.</exception>
        public double Y
        {
            get => _y;
            set => SetCore(value, MinYCoordinate, MaxYCoordinate, ref _y);
        }

        /// <summary>
        /// Adds two coordinates.
        /// </summary>
        /// <param name="first">The first operand.</param>
        /// <param name="second">The second operand.</param>
        /// <returns>The sum of <paramref name="first"/> and <paramref name="second"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown, when resulted X is not in [-90; 90] or resulted Y is not in [0; 180].</exception>
        public static Coordinates operator +(Coordinates first, Coordinates second)
        {
            return ArithmeticOperationCore(first, second, (x, y) => x + y);
        }

        /// <summary>
        /// Subtracts two coordinates.
        /// </summary>
        /// <param name="first">The first operand.</param>
        /// <param name="second">The second operand.</param>
        /// <returns>The subtract from <paramref name="first"/> of <paramref name="second"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown, when resulted X is not in [-90; 90] or resulted Y is not in [0; 180].</exception>
        public static Coordinates operator -(Coordinates first, Coordinates second)
        {
            return ArithmeticOperationCore(first, second, (x, y) => x - y);
        }

        /// <summary>
        /// Makes equality comparison of two coordinates.
        /// </summary>
        /// <param name="first">The first operand.</param>
        /// <param name="second">The second operand.</param>
        /// <returns><see langword="true" /> if the <paramref name="first"/> is equal to the <paramref name="second" />; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(Coordinates first, Coordinates second) => first.Equals(second);

        /// <summary>
        /// Makes inequality comparison of two coordinates.
        /// </summary>
        /// <param name="first">The first operand.</param>
        /// <param name="second">The second operand.</param>
        /// <returns><see langword="true" /> if the <paramref name="first"/> is not equal to the <paramref name="second" />; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(Coordinates first, Coordinates second) => !(first == second);

        /// <summary>
        /// Indicates whether the current coordinates is equal to another coordinates.
        /// </summary>
        /// <param name="other">Coordinates to compare with this coordinates.</param>
        /// <returns> <see langword="true" /> if the current coordinates is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(Coordinates other)
        {
            return X.CompareTo(other.X, Precision) == 0 && Y.CompareTo(other.Y, Precision) == 0;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return obj is Coordinates other && this.Equals(other);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return (X.GetHashCode() * 397) ^ Y.GetHashCode();
        }

        /// <summary>
        /// Returns the representation of this instance.
        /// </summary>
        /// <returns>The the representation of this instance.</returns>
        public override string ToString()
        {
            return $"Coordinates: X = {X.ToString()}, Y = {Y.ToString()}";
        }

        private void SetCore(double value, double minCoordinate, double maxCoordinate, ref double toBeSet)
        {
            if (value < minCoordinate || value > maxCoordinate)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"The value is not in [{minCoordinate.ToString()}; {maxCoordinate.ToString()}].");
            }

            toBeSet = value;
        }

        private static Coordinates ArithmeticOperationCore(Coordinates first, Coordinates second, Func<double, double, double> arithmeticOperation)
        {
            try
            {
                return new Coordinates(arithmeticOperation(first.X, second.X), arithmeticOperation(first.Y, second.Y));
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new InvalidOperationException($"The result of arithmetic operation of {first} and {second} is out of range for coordinates value", e);
            }
        }
    }
}
