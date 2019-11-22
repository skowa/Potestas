using Potestas.Configuration;
using Potestas.Observations;

namespace Potestas.SqlPlugin.Processors
{
    public class SaveFlashObservationToSqlProcessor : BaseSaveToSqlProcessor<FlashObservation>
    {
        public SaveFlashObservationToSqlProcessor(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string GetQueryString(FlashObservation value) =>
            FlashObservationQueries.CreateInsertQuery(value);
    }
}