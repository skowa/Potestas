using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Potestas.CodeFirst.Entities
{
    public class FlashObservation
    {
        public int Id { get; set; }

        [Column(TypeName = "geometry")]
        [Required]
        public Point ObservationPoint { get; set; }

        public double Intensity { get; set; }

        public int DurationMs { get; set; }

        public DateTime ObservationTime { get; set; }

        public double EstimatedValue { get; set; }
    }
}