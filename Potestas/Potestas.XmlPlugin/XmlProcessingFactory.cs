using System.IO;
using Potestas.Configuration;
using Potestas.Processors;

namespace Potestas.XmlPlugin
{
    public class XmlProcessingFactory<T> : IProcessingFactory<T> where T : IEnergyObservation
    {
        private readonly IConfiguration _configuration;

        public XmlProcessingFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnergyObservationProcessor<T> CreateProcessor()
        {
            string filePath = this.GetFilePath();
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return new SaveToFileProcessor<T>(new SerializeToXmlProcessor<T>(), filePath);
        }

        public IEnergyObservationStorage<T> CreateStorage() => new XmlFileStorage<T>(this.GetFilePath());

        public IEnergyObservationAnalizer CreateAnalizer() => new XmlAnalyzer<T>(this.GetFilePath());

        private string GetFilePath()
        {
            return _configuration.GetValue("xmlFilePath");
        }
    }
}