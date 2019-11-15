using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Potestas.Comparers;

namespace Potestas.XmlPlugin
{
    public class XmlAnalyzer<T> : IEnergyObservationAnalizer where T : IEnergyObservation
    {
        private const string EstimatedValue = "EstimatedValue";
        private const string ObservationTime = "ObservationTime";
        private const string ObservationPoint = "Coordinates";

        private readonly string _filePath;
        private IEnumerable<XElement> _elements;
        private bool _isDocLoaded = false;

        public XmlAnalyzer(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            _filePath = filePath;
        }

        public XmlAnalyzer(XDocument xDocument)
        {
            if (xDocument == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            if (xDocument.Root == null)
            {
                throw new ArgumentException($"{nameof(xDocument)} is empty. There is no data to analyze.");
            }

            _isDocLoaded = true;
            _elements = this.GetElements(xDocument);
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            return this.GetElements().GroupBy(e => e.FromXElement<T>(), new EstimatedValueEqualityComparer(new Configuration.Configuration()))
                .ToDictionary(group => group.Key.EstimatedValue, group => group.Count());
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            return this.GetElements().GroupBy(e => e.FromXElement<T>(), new ObservationPointEqualityComparer())
                .ToDictionary(group => group.Key.ObservationPoint, group => group.Count());
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            return this.GetElements().GroupBy(e => e.FromXElement<T>(), new ObservationTimeEqualityComparer())
                .ToDictionary(group => group.Key.ObservationTime, group => group.Count());
        }

        public double GetMaxEnergy()
        {
            return this.GetElements().Max(e => (double) e.Element(EstimatedValue));
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            return this.GetElements().Where(e => e.Element(ObservationPoint).FromXElement<Coordinates>() == coordinates)
                .Max(e => (double) e.Element(EstimatedValue));
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            return this.GetElements().Where(e => (DateTime) e.Element(ObservationTime) == dateTime)
                .Max(e => (double) e.Element(EstimatedValue));
        }

        public double GetMinEnergy()
        {
            return this.GetElements().Min(e => (double) e.Element(EstimatedValue));
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            return this.GetElements().Where(e => e.Element(ObservationPoint).FromXElement<Coordinates>() == coordinates)
                .Min(e => (double) e.Element(EstimatedValue));
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            return this.GetElements().Where(e => (DateTime) e.Element(ObservationTime) == dateTime)
                .Min(e => (double) e.Element(EstimatedValue));
        }

        public double GetAverageEnergy()
        {
            return this.GetElements().Average(e => (double)e.Element(EstimatedValue));
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            return this.GetElements()
                .Where(e =>
                {
                    var observationTime = (DateTime) e.Element(ObservationTime);

                    return observationTime >= startFrom &&
                           observationTime <= endBy;
                }).Average(e => (double)e.Element(EstimatedValue));
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            return this.GetElements().Where(e =>
                {
                    double x = e.Element(ObservationPoint).FromXElement<Coordinates>().X;
                    double y = e.Element(ObservationPoint).FromXElement<Coordinates>().Y;

                    return x >= rectTopLeft.X &&
                           x <= rectBottomRight.X &&
                           y <= rectTopLeft.Y &&
                           y >= rectBottomRight.Y;
                })
                .Average(e => (double)e.Element(EstimatedValue));
        }

        public DateTime GetMaxEnergyTime()
        {
            return this.GetElements().Max(e => (DateTime) e.Element(ObservationTime));
        }

        public Coordinates GetMaxEnergyPosition()
        {
            return this.GetElements().Select(e => e.Element(ObservationPoint).FromXElement<Coordinates>())
                .OrderBy(e => e, new CoordinatesComparer(new Configuration.Configuration()))
                .Last();
        }

        public DateTime GetMinEnergyTime()
        {
            return this.GetElements().Min(e => (DateTime)e.Element(ObservationTime));
        }

        public Coordinates GetMinEnergyPosition()
        {
            return this.GetElements().Select(e => e.Element(ObservationPoint).FromXElement<Coordinates>())
                .OrderBy(e => e, new CoordinatesComparer(new Configuration.Configuration()))
                .First();
        }

        private IEnumerable<XElement> GetElements()
        {
            if (!_isDocLoaded)
            {
                var xDocument = XDocument.Load(_filePath);
                _isDocLoaded = true;
                _elements = this.GetElements(xDocument);
            }

            return _elements;
        }

        private IEnumerable<XElement> GetElements(XDocument xDocument)
        {
            return xDocument.Root?.Elements(typeof(T).Name);
        }
    }
}