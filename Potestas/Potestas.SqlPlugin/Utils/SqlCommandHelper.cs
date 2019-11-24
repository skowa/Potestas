using System.Data.SqlClient;
using Potestas.Observations;
using Potestas.SqlPlugin.Mappers;

namespace Potestas.SqlPlugin.Utils
{
    internal static class SqlCommandHelper
    {
        internal static void FillParam<T>(SqlCommand sqlCommand, string paramName, T value)
        {
            sqlCommand.Parameters.AddWithValue(paramName, value);
        }

        internal static void FillParam(SqlCommand sqlCommand, string paramName, Coordinates coordinates)
        {
            sqlCommand.Parameters.Add(new SqlParameter(paramName, coordinates.ToSqlGeometry()) { UdtTypeName = "Geometry" });
        }

        internal static void FillFlashObservation(SqlCommand sqlCommand, FlashObservation value)
        {
            FillParam(sqlCommand, "@ObservationPoint", value.ObservationPoint);
            FillParam(sqlCommand, "@ObservationTime", value.ObservationTime);
            FillParam(sqlCommand, "@DurationMs", value.DurationMs);
            FillParam(sqlCommand, "@Intensity", value.Intensity);
            FillParam(sqlCommand, "@EstimatedValue", value.EstimatedValue);
        }
    }
}