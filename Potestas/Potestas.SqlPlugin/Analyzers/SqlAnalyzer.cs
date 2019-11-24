using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Types;
using Potestas.Configuration;
using Potestas.SqlHelper;
using Potestas.SqlPlugin.Utils;

namespace Potestas.SqlPlugin.Analyzers
{
    public class SqlAnalyzer : IEnergyObservationAnalizer
    {
        private readonly string _connectionString;

        public SqlAnalyzer(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue("connectionString") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            return this.ExecuteReaderStoredProcedure<double, int>("dbo.GetDistributionByEnergyValue", "EstimatedValue", "Count");
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            return this.ExecuteReaderStoredProcedureWithCoordinatesKey<int>("dbo.GetDistributionByCoordinates", "Coordinates", "Count");
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            return this.ExecuteReaderStoredProcedure<DateTime, int>("dbo.GetDistributionByObservationTime", "ObservationTime", "Count");
        }

        public double GetMaxEnergy() => this.ExecuteScalarStoredProcedure<double>("dbo.GetMaxEnergy");

        public double GetMaxEnergy(Coordinates coordinates)
        {
            return this.ExecuteScalarStoredProcedure<double>("dbo.GetMaxEnergy_Coordinates",
                command => SqlCommandHelper.FillParam(command, "@Coordinates", coordinates));
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            return this.ExecuteScalarStoredProcedure<double>("dbo.GetMaxEnergy_DateTime",
                command => SqlCommandHelper.FillParam(command, "@DateTime", dateTime));
        }

        public double GetMinEnergy() => this.ExecuteScalarStoredProcedure<double>("dbo.GetMinEnergy");

        public double GetMinEnergy(Coordinates coordinates)
        {
            return this.ExecuteScalarStoredProcedure<double>("dbo.GetMinEnergy_Coordinates",
                command => SqlCommandHelper.FillParam(command, "@Coordinates", coordinates));
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            return this.ExecuteScalarStoredProcedure<double>("dbo.GetMinEnergy_DateTime",
                command => SqlCommandHelper.FillParam(command, "@DateTime", dateTime));
        }

        public double GetAverageEnergy() => this.ExecuteScalarStoredProcedure<double>("dbo.GetAverageEnergy");

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            return this.ExecuteScalarStoredProcedure<double>("dbo.GetAverageEnergy_StartFrom_EndBy",
                command =>
                {
                    SqlCommandHelper.FillParam(command, "@StartFrom", startFrom);
                    SqlCommandHelper.FillParam(command, "@EndBy", endBy);
                });
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            return this.ExecuteScalarStoredProcedure<double>("dbo.GetAverageEnergy_RectTopLeft_RectBottomRight",
                command =>
                {
                    SqlCommandHelper.FillParam(command, "@RectTopLeft", rectTopLeft);
                    SqlCommandHelper.FillParam(command, "@RectBottomRight", rectBottomRight);
                });
        }

        public DateTime GetMaxEnergyTime() => this.ExecuteScalarStoredProcedure<DateTime>("dbo.GetMaxEnergyTime");

        public Coordinates GetMaxEnergyPosition() => 
            this.ExecuteScalarStoredProcedure<SqlGeometry>("dbo.GetMaxEnergyPosition").ToCoordinates();

        public DateTime GetMinEnergyTime() => this.ExecuteScalarStoredProcedure<DateTime>("dbo.GetMinEnergyTime");

        public Coordinates GetMinEnergyPosition() =>
            this.ExecuteScalarStoredProcedure<SqlGeometry>("dbo.GetMinEnergyPosition").ToCoordinates();

        private IDictionary<TKey, TValue> ExecuteReaderStoredProcedure<TKey, TValue>(string storedProcedureName, string firstParameter, string secondParameter)
        {
            return ExecuteReaderStoredProcedureCore(storedProcedureName, firstParameter, secondParameter,
                TransformValues<TKey, TValue>);
        }

        private IDictionary<Coordinates, TValue> ExecuteReaderStoredProcedureWithCoordinatesKey<TValue>(string storedProcedureName, string firstParameter, string secondParameter)
        {
            return ExecuteReaderStoredProcedureCore(storedProcedureName, firstParameter, secondParameter,
                TransformValuesWithCoordinatesKey<TValue>);
        }

        private IDictionary<TKey, TValue> ExecuteReaderStoredProcedureCore<TKey, TValue>(string storedProcedureName, string firstParameter, string secondParameter, Func<SqlDataReader, string, string, (TKey, TValue)> transformValues)
        {
            return this.CreateCommandCore(storedProcedureName, command =>
            {
                using SqlDataReader reader = command.ExecuteReader();
                var result = new Dictionary<TKey, TValue>();
                while (reader.Read())
                {
                    var (key, value) = transformValues(reader, firstParameter, secondParameter);
                    result.Add(key, value);
                }

                return result;
            });
        }

        private (TKey, TValue) TransformValues<TKey, TValue>(SqlDataReader reader, string firstParameter, string secondParameter)
        {
            return ((TKey)reader[firstParameter], (TValue)reader[secondParameter]);
        }

        private (Coordinates, TValue) TransformValuesWithCoordinatesKey<TValue>(SqlDataReader reader, string firstParameter, string secondParameter)
        {
            return (((SqlGeometry)reader[firstParameter]).ToCoordinates(), (TValue)reader[secondParameter]);
        }

        private T ExecuteScalarStoredProcedure<T>(string storedProcedureName)
        {
            return CreateCommandCore(storedProcedureName, command => (T) command.ExecuteScalar());
        }

        private T ExecuteScalarStoredProcedure<T>(string storedProcedureName, Action<SqlCommand> fillParameters)
        {
            return this.CreateCommandCore(storedProcedureName, command =>
            {
                fillParameters(command);
                return (T) command.ExecuteScalar();
            });
        }

        private T CreateCommandCore<T>(string storedProcedureName, Func<SqlCommand, T> executeCommand)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var command = sqlConnection.CreateCommand();
            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;
            sqlConnection.Open();
               
            return executeCommand(command);
        }

        
    }
}