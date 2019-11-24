namespace Potestas.SqlHelper
{
    public static class FlashObservationQueries
    {
        private static readonly string _tableName = "dbo.FlashObservations";
        
        public static string CreateInsertQuery()
        {
            return $@"INSERT INTO {_tableName} (Intensity, DurationMs, ObservationPoint, ObservationTime, EstimatedValue)
                                    VALUES (@Intensity, @DurationMs, @ObservationPoint, @ObservationTime, @EstimatedValue);";
        }

        public static string CreateDeleteQuery()
        {
            return $"DELETE FROM {_tableName} WHERE Id = @Id";
        }

        public static string CreateGetAllQuery() => $"SELECT * FROM {_tableName};";

        public static string CreateGetCountQuery() => $"SELECT COUNT(*) FROM {_tableName}";

        public static string CreateDeleteAllQuery() => $"DELETE FROM {_tableName}";
    }
}