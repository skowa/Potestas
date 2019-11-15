using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Potestas.XmlPlugin
{
    public static class XElementMapper
    {
        public static XElement ToXElement<T>(this T obj)
        {
            var xmlSerializer = new XmlSerializer<T>();

            using var memoryStream = new MemoryStream();
            using var streamWriter = XmlWriter.Create(memoryStream);
            xmlSerializer.Serialize(streamWriter, obj);

            return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
        }

        public static T FromXElement<T>(this XElement xElement)
        {
            var xmlSerializer = new XmlSerializer<T>();

            return xmlSerializer.Deserialize(xElement.CreateReader());
        }
    }
}