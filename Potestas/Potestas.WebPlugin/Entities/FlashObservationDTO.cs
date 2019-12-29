using System;

namespace Potestas.WebPlugin.Entities
{
	public class FlashObservationDTO
	{
		public int Id { get; set; }

		public CoordinatesDTO ObservationPoint { get; set; }

		public double Intensity { get; set; }

		public int DurationMs { get; set; }

		public DateTime ObservationTime { get; set; }

		public double EstimatedValue { get; set; }
    }
}