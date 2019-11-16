using System.Globalization;
using Potestas.Configuration;
using Potestas.Observations;

namespace Potestas.SqlPlugin.Processors
{
    public class SaveFlashObservationToSqlProcessor : BaseSaveToSqlProcessor<FlashObservation>
    {
        public SaveFlashObservationToSqlProcessor(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string GetQueryString(FlashObservation value)
        {
            var intensity = value.Intensity.ToString(CultureInfo.InvariantCulture);
            var durationMs = value.DurationMs.ToString();
            var observationPoint = $@"geometry::STPointFromText('POINT ({value.ObservationPoint.X.ToString(CultureInfo.InvariantCulture)} 
                                                                        {value.ObservationPoint.X.ToString(CultureInfo.InvariantCulture)})', 0)";
            var observationTime = value.ObservationTime.ToString(CultureInfo.InvariantCulture);
            var estimatedValue = value.EstimatedValue.ToString(CultureInfo.InvariantCulture);

            return $@"INSERT INTO dbo.FlashObservations (Intensity, DurationMs, ObservationPoint, ObservationTime, EstimatedValue)
                                    VALUES ({intensity}, {durationMs}, {observationPoint}, '{observationTime}', {estimatedValue});";
        }
    }
}