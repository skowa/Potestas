using System;

namespace Potestas.Web.Models
{
	public class FlashObservationModel
	{
		public int Id { get; set; }

		public CoordinatesModel ObservationPoint { get; set; }

		public double Intensity { get; set; }

		public int DurationMs { get; set; }

		public DateTime ObservationTime { get; set; }

		public double EstimatedValue { get; set; }
	}
}