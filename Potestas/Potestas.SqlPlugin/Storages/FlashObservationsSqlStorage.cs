using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Potestas.Configuration;
using Potestas.Observations;
using Potestas.SqlPlugin.Mappers;
using Potestas.SqlPlugin.Utils;

namespace Potestas.SqlPlugin.Storages
{
    public class FlashObservationsSqlStorage : BaseSqlStorage<FlashObservation>,
        IEnergyObservationStorage<FlashObservation>, ICollection<FlashObservation>, IEnumerable<FlashObservation>, IEnumerable
    {
        public FlashObservationsSqlStorage(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string GetSelectAllQuery() => FlashObservationQueries.CreateGetAllQuery();

        protected override SqlCommand GetInsertCommand(FlashObservation value)
        {
            var sqlCommand = new SqlCommand(FlashObservationQueries.CreateInsertQuery());
            SqlCommandHelper.FillFlashObservation(sqlCommand, value);

            return sqlCommand;
        }

        protected override SqlCommand GetDeleteCommand(int id)
        {
            var sqlCommand = new SqlCommand(FlashObservationQueries.CreateDeleteQuery());
            SqlCommandHelper.FillParam(sqlCommand, "@Id", id);

            return sqlCommand;
        }

        protected override SqlCommand GetCountCommand() => new SqlCommand(FlashObservationQueries.CreateGetCountQuery());

        protected override FlashObservation DataRowToObservations(DataRow dataRow)
        {
            return dataRow.ToFlashObservation();
        }
    }
}