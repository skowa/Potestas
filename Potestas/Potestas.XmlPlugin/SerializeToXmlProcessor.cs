using System;
using System.Xml;
using Potestas.Processors;

namespace Potestas.XmlPlugin
{
    public class SerializeToXmlProcessor<T> : SerializeProcessor<T>, IDisposable, IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly XmlWriter _xmlWriter;
        private bool _isDisposed;

        public SerializeToXmlProcessor() : base(new XmlSerializer<T>())
        {
            var rootName = $"{typeof(T).Name}s";

            var xmlSettings = new XmlWriterSettings
            {
                Indent = true
            };
            _xmlWriter = XmlWriter.Create(Stream, xmlSettings);
            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteStartElement(rootName);
        }

        public override void Dispose()
        {
            if (!_isDisposed)
            {
                _xmlWriter.Dispose();
                base.Dispose();
                _isDisposed = true;
            }
        }

        public override void OnCompleted()
        {
            this.EndDocument();

            base.OnCompleted();
        }

        public override void OnError(Exception error)
        {
            this.EndDocument();

            base.OnError(error);
        }

        protected override void SerializeToStream(T value)
        {
            (Serializer as XmlSerializer<T>)?.Serialize(_xmlWriter, value);
        }

        private void EndDocument()
        {
            _xmlWriter.WriteEndElement();
            _xmlWriter.WriteEndDocument();
            _xmlWriter.Flush();
        }
    }
}
