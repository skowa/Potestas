using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Potestas.Processors;
using Potestas.Utils;

namespace Potestas.XmlPlugin
{
    public class XmlSerializer<T> : ISerializer<T>
    {
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(T));

        public void Serialize(Stream stream, T value)
        {
            this.SerializeArgumentsCheck(stream, value);

            _xmlSerializer.Serialize(stream, value);
        }

        public void Serialize(XmlWriter xmlWriter, T value)
        {
            this.SerializeArgumentsCheck(xmlWriter, value);

            _xmlSerializer.Serialize(xmlWriter, value);
        }

        public T Deserialize(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var value = (T) _xmlSerializer.Deserialize(stream);

            return value;
        }

        public T Deserialize(XmlReader xmlReader)
        {
            if (xmlReader == null)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            var value = (T)_xmlSerializer.Deserialize(xmlReader);

            return value;
        }

        private void SerializeArgumentsCheck<TStream>(TStream stream, T value)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (Validator.IsGenericTypeNull(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
        }
    }
}