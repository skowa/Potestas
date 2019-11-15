using System.IO;
using System.Xml.Linq;
using Potestas.Observations;
using Potestas.Processors;
using Potestas.XmlPlugin;

namespace Potestas.Tests.Analizers
{
    public class XmlAnalyzerTests : BaseAnalyzerTests
    {
        private readonly string _filePath = "xmlAnalyzerTests.xml";

        protected override IEnergyObservationAnalizer GetAnalyzer()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            using (var fileProcessor = new SaveToFileProcessor<FlashObservation>(new SerializeToXmlProcessor<FlashObservation>(), _filePath))
            {
                foreach (var flashObservation in ListStorage)
                {
                    fileProcessor.OnNext(flashObservation);
                }

                fileProcessor.OnCompleted();
            }

            return new XmlAnalyzer<FlashObservation>(XDocument.Load(_filePath));
        }
    }
}