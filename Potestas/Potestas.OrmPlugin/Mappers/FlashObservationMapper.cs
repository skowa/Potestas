using Potestas.Observations;
using FlashObservationDTO = Potestas.CodeFirst.Entities.FlashObservation;

namespace Potestas.OrmPlugin.Mappers
{
    internal static class FlashObservationMapper
    {
        internal static FlashObservation ToFlashObservation(this FlashObservationDTO flashObservationDto)
        {
            return new FlashObservation(flashObservationDto.Id, flashObservationDto.ObservationPoint.ToCoordinates(),
                flashObservationDto.Intensity, flashObservationDto.DurationMs, flashObservationDto.ObservationTime);
        }

        internal static FlashObservationDTO ToFlashObservationDTO(this FlashObservation flashObservation)
        {
            return new FlashObservationDTO
            {
                Id = flashObservation.Id,
                DurationMs = flashObservation.DurationMs,
                EstimatedValue = flashObservation.EstimatedValue,
                Intensity = flashObservation.Intensity,
                ObservationPoint = flashObservation.ObservationPoint.ToPoint(),
                ObservationTime = flashObservation.ObservationTime
            };
        }
    }
}