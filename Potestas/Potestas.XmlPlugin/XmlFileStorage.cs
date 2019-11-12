using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Potestas.Storages;
using Potestas.Utils;

namespace Potestas.XmlPlugin
{
    public class XmlFileStorage<T> : BaseFileStorage<T>, IEnergyObservationStorage<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : IEnergyObservation
    {
        private readonly string _filePath;

        public XmlFileStorage(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            _filePath = filePath;
        }

        public string Description => "Stores energy observations in provided xml file";

        public int Count
        {
            get
            {
                var xPathDocument = new XPathDocument(_filePath);
                XPathNavigator navigator = xPathDocument.CreateNavigator();
                var count = Convert.ToInt32(navigator.Evaluate("count(//FlashObservation)"));

                return count;
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (Validator.IsGenericTypeNull(item))
            {
                throw new ArgumentNullException(nameof(item));
            }

            var document = XDocument.Load(_filePath);
            document.Root?.Add(item.ToXElement());
            document.Save(_filePath);
        }

        public void Clear()
        {
            var document = XDocument.Load(_filePath);
            document.Root?.RemoveAll();
            document.Save(_filePath);
        }

        public bool Contains(T item) => this.Contains(item, this);

        public void CopyTo(T[] array, int arrayIndex) => this.CopyTo(array, arrayIndex, this);

        public IEnumerator<T> GetEnumerator()
        {
            var xmlSerializer = new XmlSerializer<T>();
            var xmlReaderSettings = new XmlReaderSettings
            {
                IgnoreWhitespace = true
            };
            using var xmlReader = XmlReader.Create(_filePath, xmlReaderSettings);
            if (xmlReader.EOF)
            {
                yield break;
            }

            xmlReader.ReadToFollowing(typeof(T).Name);
            while (xmlReader.NodeType == XmlNodeType.Element)
            {
                yield return xmlSerializer.Deserialize(xmlReader);
            }

            xmlReader.ReadEndElement();
        }

        public bool Remove(T item)
        {
            if (Validator.IsGenericTypeNull(item))
            {
                throw new ArgumentNullException(nameof(item));
            }

            var document = XDocument.Load(_filePath); 
            var xElement = document.Root?.Elements().FirstOrDefault(e => e.FromXElement<T>().Equals(item));
            if (xElement == null)
            {
                return false;
            }

            xElement.Remove();
            document.Save(_filePath);

            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}