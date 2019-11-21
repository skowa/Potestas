using System;
using System.Data;
using Microsoft.SqlServer.Types;
using Potestas.Observations;

namespace Potestas.SqlPlugin.Mappers
{
    internal static class FlashObservationMappers
    {
        internal static FlashObservation ToFlashObservation(this DataRow dataRow)
        {
            var id = (int) dataRow["Id"];
            var coordinates = (SqlGeometry) dataRow["ObservationPoint"];
            var observationPoint = new Coordinates(coordinates.STX.Value, coordinates.STY.Value);
            var intensity = (double) dataRow["Intensity"];
            var durationMs = (int) dataRow["DurationMs"];
            var observationTime = (DateTime) dataRow["ObservationTime"];
           
            return new FlashObservation(id, observationPoint, intensity, durationMs, observationTime);
        }
    }
}