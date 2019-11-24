using Potestas.Configuration;
using Potestas.Observations;
using Potestas.SqlHelper;

namespace Potestas.OrmPlugin.Processors
{
    public class SaveFlashObservationToSqlProcessor : BaseSaveToSqlProcessor<FlashObservation>
    {
        public SaveFlashObservationToSqlProcessor(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string GetQueryString(FlashObservation value) => FlashObservationQueries.CreateInsertQuery();
    }
}