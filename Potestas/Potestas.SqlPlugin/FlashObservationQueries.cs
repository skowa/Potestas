using System.Data.SqlClient;
using System.Globalization;
using Potestas.Observations;

namespace Potestas.SqlPlugin
{
    public static class FlashObservationQueries
    {
        private static readonly string _tableName = "dbo.FlashObservations";
        
        public static string CreateInsertQuery(FlashObservation value)
        {
            var intensity = value.Intensity.ToString(CultureInfo.InvariantCulture);
            var durationMs = value.DurationMs.ToString();
            var observationPoint = $@"geometry::STPointFromText('POINT ({value.ObservationPoint.X.ToString(CultureInfo.InvariantCulture)} 
                                                                        {value.ObservationPoint.Y.ToString(CultureInfo.InvariantCulture)})', 0)";
            var observationTime = value.ObservationTime.ToString(CultureInfo.InvariantCulture);
            var estimatedValue = value.EstimatedValue.ToString(CultureInfo.InvariantCulture);

            return $@"INSERT INTO {_tableName} (Intensity, DurationMs, ObservationPoint, ObservationTime, EstimatedValue)
                                    VALUES ({intensity}, {durationMs}, {observationPoint}, '{observationTime}', {estimatedValue});";
        }

        public static string CreateDeleteQuery(int id)
        {
            return $"DELETE FROM {_tableName} WHERE Id = {id.ToString()}";
        }

        public static string CreateGetAllQuery() => $"SELECT * FROM {_tableName};";

        public static string CreateGetCountQuery() => $"SELECT COUNT(*) FROM {_tableName}";
    }
}