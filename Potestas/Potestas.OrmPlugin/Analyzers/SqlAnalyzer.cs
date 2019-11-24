using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.SqlServer.Types;
using Potestas.Configuration;
using Potestas.SqlHelper;

namespace Potestas.OrmPlugin.Analyzers
{
    public class SqlAnalyzer : IEnergyObservationAnalizer
    {
        private readonly string _connectionString;

        public SqlAnalyzer(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue("connectionString") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IDictionary<double, int> GetDistributionByEnergyValue() =>
            this.ExecuteStoredProcedureAsDictionary<double, int>("dbo.GetDistributionByEnergyValue", row => row.EstimatedValue, row => row.Count);

        public IDictionary<Coordinates, int> GetDistributionByCoordinates() =>
            this.ExecuteStoredProcedureAsDictionary<Coordinates, int>("dbo.GetDistributionByCoordinates", row => ((SqlGeometry)row.Coordinates).ToCoordinates(), row => row.Count);

        public IDictionary<DateTime, int> GetDistributionByObservationTime() =>
            this.ExecuteStoredProcedureAsDictionary<DateTime, int>("dbo.GetDistributionByObservationTime", row => row.ObservationTime, row => row.Count);

        public double GetMaxEnergy() => this.ExecuteStoredProcedureAsScalar<double>("dbo.GetMaxEnergy");

        public double GetMaxEnergy(Coordinates coordinates) =>
            this.ExecuteStoredProcedureAsScalar<double>("dbo.GetMaxEnergy_Coordinates", new {coordinates});

        public double GetMaxEnergy(DateTime dateTime) =>
            this.ExecuteStoredProcedureAsScalar<double>("dbo.GetMaxEnergy_DateTime", new {dateTime});

        public double GetMinEnergy() => this.ExecuteStoredProcedureAsScalar<double>("dbo.GetMinEnergy");

        public double GetMinEnergy(Coordinates coordinates) =>
            this.ExecuteStoredProcedureAsScalar<double>("dbo.GetMinEnergy_Coordinates", new { coordinates });

        public double GetMinEnergy(DateTime dateTime) =>
            this.ExecuteStoredProcedureAsScalar<double>("dbo.GetMinEnergy_DateTime", new { dateTime });

        public double GetAverageEnergy() => this.ExecuteStoredProcedureAsScalar<double>("dbo.GetAverageEnergy");

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy) =>
            this.ExecuteStoredProcedureAsScalar<double>("dbo.GetAverageEnergy_StartFrom_EndBy", new {startFrom, endBy});

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight) =>
            this.ExecuteStoredProcedureAsScalar<double>("dbo.GetAverageEnergy_RectTopLeft_RectBottomRight", new { rectTopLeft, rectBottomRight });

        public DateTime GetMaxEnergyTime() => this.ExecuteStoredProcedureAsScalar<DateTime>("dbo.GetMaxEnergyTime");

        public Coordinates GetMaxEnergyPosition() => this.ExecuteStoredProcedureAsScalar<Coordinates>("dbo.GetMaxEnergyPosition");

        public DateTime GetMinEnergyTime() => this.ExecuteStoredProcedureAsScalar<DateTime>("dbo.GetMinEnergyTime");

        public Coordinates GetMinEnergyPosition() => this.ExecuteStoredProcedureAsScalar<Coordinates>("dbo.GetMinEnergyPosition");

        private T ExecuteStoredProcedureAsScalar<T>(string procedureName, object param = null)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.ExecuteScalar<T>(procedureName, param, commandType: CommandType.StoredProcedure);
        }

        private IDictionary<TKey, TValue> ExecuteStoredProcedureAsDictionary<TKey, TValue>(string procedureName, Func<dynamic, dynamic> getKey, Func<dynamic, dynamic> getValue)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query(procedureName, commandType: CommandType.StoredProcedure).ToDictionary(row => (TKey)getKey(row), row => (TValue)getValue(row));
        }
    }
}