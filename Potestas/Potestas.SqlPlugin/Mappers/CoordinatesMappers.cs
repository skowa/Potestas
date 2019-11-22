using Microsoft.SqlServer.Types;

namespace Potestas.SqlPlugin.Mappers
{
    internal static class CoordinatesMappers
    {
        internal static SqlGeometry ToSqlGeometry(this Coordinates coordinates) =>
            SqlGeometry.Point(coordinates.X, coordinates.Y, 0);

        internal static Coordinates ToCoordinates(this SqlGeometry geometry) =>
            new Coordinates(geometry.STX.Value, geometry.STY.Value);
    }
}