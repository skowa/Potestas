using NetTopologySuite.Geometries;

namespace Potestas.OrmPlugin.Mappers
{
    internal static class CoordinatesMapper
    {
        internal static Coordinates ToCoordinates(this Point point)
        {
            return new Coordinates(point.X, point.Y);
        }

        internal static Point ToPoint(this Coordinates coordinates)
        {
            return new Point(coordinates.X, coordinates.Y);
        }
    }
}