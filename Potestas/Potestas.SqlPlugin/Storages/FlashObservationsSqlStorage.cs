using System.Collections;
using System.Collections.Generic;
using System.Data;
using Potestas.Configuration;
using Potestas.Observations;
using Potestas.SqlPlugin.Mappers;

namespace Potestas.SqlPlugin.Storages
{
    public class FlashObservationsSqlStorage : BaseSqlStorage<FlashObservation>,
        IEnergyObservationStorage<FlashObservation>, ICollection<FlashObservation>, IEnumerable<FlashObservation>, IEnumerable
    {
        public FlashObservationsSqlStorage(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string GetSelectAllQuery() => FlashObservationQueries.CreateGetAllQuery();

        protected override string GetInsertQuery(FlashObservation value) =>
            FlashObservationQueries.CreateInsertQuery(value);

        protected override string GetDeleteQuery(int id) =>
            FlashObservationQueries.CreateDeleteQuery(id);

        protected override string GetCountQuery() => FlashObservationQueries.CreateGetCountQuery();

        protected override FlashObservation DataRowToObservations(DataRow dataRow)
        {
            return dataRow.ToFlashObservation();
        }
    }
}