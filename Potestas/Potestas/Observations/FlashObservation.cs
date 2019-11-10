using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Potestas.ExtensionMethods;

namespace Potestas.Observations
{
    /* TASK: Implement this structure by following requirements:
    * 1. EstimatedValue is the intensity multiple by duration
    * 2. Observations are equal if they made at the same time, 
    * the same observation point and EstimatedValue 
    * is the same by decimal presicion
    * 3. Implement custom constructors with ability to set ObservationTime by moment of creation or from constructor parameter.
    * 4. Implement == and != operators for the structure.
    * 6. Negative Intensity is a sign of invalid observation. Figure out how to process such errors. Remember you are writing a library.
    * 7. Intensity more than 2 000 000 000 is imposible and could be a sign of the invalid observation.
    * 8. Implement nice string representation of this observation.
    * QUESTIONS: 
    * How implementation of interface impacts boxing and unboxing operation for the structure?
    * Why overriding of Equals method is not enough?
    * What kind of pollymorhism does this struct contain?
    * Why immutable structure is used here?
    * TESTS: Cover this structure with unit tests
    */
    [Serializable]
    public struct FlashObservation : IEnergyObservation, IEquatable<FlashObservation>, IXmlSerializable
    {
        private const double Precision = 0.1;

        public FlashObservation(Coordinates observationPoint, double intensity, int durationMs) 
            : this(observationPoint, intensity, durationMs, DateTime.Now)
        {

        }

        public FlashObservation(Coordinates observationPoint, double intensity, int durationMs, DateTime observationTime) : this()
        {
            if (intensity < MinIntensityValue || intensity > MaxIntensityValue)
            {
                throw new ArgumentOutOfRangeException(nameof(intensity), $"The intensity is not in [{MinIntensityValue.ToString()}; {MaxIntensityValue.ToString()}]");
            }

            Intensity = intensity;
            ObservationPoint = observationPoint;
            DurationMs = durationMs;
            ObservationTime = observationTime;
        }

        public Coordinates ObservationPoint { get; }

        public double Intensity { get; }

        public int DurationMs { get; }

        public DateTime ObservationTime { get; }

        public double EstimatedValue => Intensity * DurationMs;
       
        public static double MinIntensityValue { get; } = 0;
        
        public static double MaxIntensityValue { get; } = 2000000000;

        public static bool operator ==(FlashObservation first, FlashObservation second) => first.Equals(second);

        public static bool operator !=(FlashObservation first, FlashObservation second) => !(first == second);

        public bool Equals(FlashObservation other)
        {
            return ObservationPoint == other.ObservationPoint && ObservationTime == other.ObservationTime &&
                   EstimatedValue.CompareTo(other.EstimatedValue, Precision) == 0;
        }

        public override bool Equals(object obj)
        {
            return obj is FlashObservation other && Equals(other);
        }

        public override int GetHashCode()
        {
            var hashCode = ObservationPoint.GetHashCode();
            hashCode = (hashCode * 397) ^ EstimatedValue.GetHashCode();
            hashCode = (hashCode * 397) ^ ObservationTime.GetHashCode();

            return hashCode;
        }

        public override string ToString()
        {
            return $"FlashObservation: ObservationPoint = {ObservationPoint.ToString()}, Intensity = {Intensity.ToString()}, " +
                   $"Duration(ms) = {DurationMs.ToString()}, ObservationTime = {ObservationTime.ToString()}, EstimatedValue = {EstimatedValue.ToString()}";
        }

        XmlSchema IXmlSerializable.GetSchema() => null;

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            var xmlSerializer = new XmlSerializer(typeof(Coordinates));
            var observationPoint = (Coordinates)xmlSerializer.Deserialize(reader);
            var intensity = reader.ReadElementContentAsDouble(nameof(Intensity), string.Empty);
            var durationMs = reader.ReadElementContentAsInt(nameof(DurationMs), string.Empty);
            DateTime.TryParse(reader.ReadElementContentAsString(nameof(ObservationTime), string.Empty), out var observationTime);
            reader.Skip();
            reader.ReadEndElement();

            this = new FlashObservation(observationPoint, intensity, durationMs, observationTime);
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            var xmlSerializer = new XmlSerializer(typeof(Coordinates));
            xmlSerializer.Serialize(writer, ObservationPoint);

            writer.WriteElementString(nameof(Intensity), Intensity.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString(nameof(DurationMs), DurationMs.ToString());
            writer.WriteElementString(nameof(ObservationTime), ObservationTime.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString(nameof(EstimatedValue), EstimatedValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}
