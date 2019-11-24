using System;
using System.Data.SqlClient;
using Dapper;
using Potestas.Configuration;
using Potestas.Utils;

namespace Potestas.OrmPlugin.Processors
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
            connection.Execute(this.GetQueryString(value), value);
        }

        public string Description => "Saves observations to database using Dapper";

        protected abstract string GetQueryString(T value);
    }
}