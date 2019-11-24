using System.Data.SqlClient;
using Potestas.Configuration;
using Potestas.Observations;
using Potestas.SqlHelper;
using Potestas.SqlPlugin.Utils;

namespace Potestas.SqlPlugin.Processors
{
    public class SaveFlashObservationToSqlProcessor : BaseSaveToSqlProcessor<FlashObservation>
    {
        public SaveFlashObservationToSqlProcessor(IConfiguration configuration) : base(configuration)
        {
        }

        protected override SqlCommand CreateInsertSqlCommand(FlashObservation value)
        {
            var sqlCommand = new SqlCommand(FlashObservationQueries.CreateInsertQuery());
            SqlCommandHelper.FillFlashObservation(sqlCommand, value);
            
            return sqlCommand;
        }
    }
}