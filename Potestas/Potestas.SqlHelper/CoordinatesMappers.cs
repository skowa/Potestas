using Microsoft.SqlServer.Types;

namespace Potestas.SqlHelper
{
    public static class CoordinatesMappers
    {
        public static SqlGeometry ToSqlGeometry(this Coordinates coordinates) =>
            SqlGeometry.Point(coordinates.X, coordinates.Y, 0);

        public static Coordinates ToCoordinates(this SqlGeometry geometry) =>
            new Coordinates(geometry.STX.Value, geometry.STY.Value);
    }
}