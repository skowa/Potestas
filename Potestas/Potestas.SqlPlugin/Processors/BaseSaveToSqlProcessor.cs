using System;
using System.Data.SqlClient;
using Potestas.Configuration;
using Potestas.Utils;

namespace Potestas.SqlPlugin.Processors
{
    public abstract class BaseSaveToSqlProcessor<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly string _connectionString;

        protected BaseSaveToSqlProcessor(IConfiguration configuration)
        {
            _connectionString = configuration?.GetValue("connectionString") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(T value)
        {
            if (Validator.IsGenericTypeNull(value))
            {
                throw new ArgumentNullException();
            }
            
            using var connection = new SqlConnection(_connectionString);
            var sqlCommand = this.CreateInsertSqlCommand(value);
            sqlCommand.Connection = connection;

            connection.Open();
            sqlCommand.ExecuteNonQuery();
        }

        public string Description => "Saves observations to database";

        protected abstract SqlCommand CreateInsertSqlCommand(T value);
    }
}