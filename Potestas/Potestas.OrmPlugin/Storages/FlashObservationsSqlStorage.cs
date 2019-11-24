using Potestas.Configuration;
using Potestas.Observations;
using Potestas.SqlHelper;

namespace Potestas.OrmPlugin.Storages
{
    public class FlashObservationsSqlStorage : BaseSqlStorage<FlashObservation>
    {
        public FlashObservationsSqlStorage(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string GetSelectAllQuery() => FlashObservationQueries.CreateGetAllQuery();

        protected override string GetInsertQuery() => FlashObservationQueries.CreateInsertQuery();

        protected override string GetDeleteQuery() => FlashObservationQueries.CreateDeleteQuery();

        protected override string GetCountQuery() => FlashObservationQueries.CreateGetCountQuery();

        protected override string GetDeleteAllQuery() => FlashObservationQueries.CreateDeleteAllQuery();
    }
}