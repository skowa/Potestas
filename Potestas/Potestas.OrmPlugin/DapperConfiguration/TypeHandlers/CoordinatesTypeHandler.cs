using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.SqlServer.Types;
using Potestas.SqlHelper;

namespace Potestas.OrmPlugin.DapperConfiguration.TypeHandlers
{
    internal class CoordinatesTypeHandler : SqlMapper.TypeHandler<Coordinates>
    {
        public override void SetValue(IDbDataParameter parameter, Coordinates value)
        {
            parameter.Value = value.ToSqlGeometry();
            if (parameter is SqlParameter sqlParameter)
            {
                sqlParameter.UdtTypeName = "geometry";
            }
        }

        public override Coordinates Parse(object value)
        {
            return (value as SqlGeometry).ToCoordinates();
        }
    }
}