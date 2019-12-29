using Potestas.Observations;
using Potestas.Web.Models;

namespace Potestas.Web.Mappers
{
	internal static class FlashObservationMapper
	{
		internal static FlashObservation ToFlashObservation(this FlashObservationModel flashObservationDto)
		{
			return new FlashObservation(flashObservationDto.Id, flashObservationDto.ObservationPoint.ToCoordinates(),
				flashObservationDto.Intensity, flashObservationDto.DurationMs, flashObservationDto.ObservationTime);
		}

		internal static FlashObservationModel ToFlashObservationModel(this FlashObservation flashObservation)
		{
			return new FlashObservationModel
			{
				Id = flashObservation.Id,
				DurationMs = flashObservation.DurationMs,
				EstimatedValue = flashObservation.EstimatedValue,
				Intensity = flashObservation.Intensity,
				ObservationPoint = flashObservation.ObservationPoint.ToCoordinatesModel(),
				ObservationTime = flashObservation.ObservationTime
			};
		}
	}
}