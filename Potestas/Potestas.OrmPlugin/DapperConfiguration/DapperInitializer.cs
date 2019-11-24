using Dapper;
using Potestas.OrmPlugin.DapperConfiguration.TypeHandlers;

namespace Potestas.OrmPlugin.DapperConfiguration
{
    public static class DapperInitializer
    {
        public static void InitDapper()
        {
            SqlMapper.AddTypeHandler(typeof(Coordinates), new CoordinatesTypeHandler());
        }
    }
}