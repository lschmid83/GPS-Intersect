using System.Xml;
using System.Xml.Serialization;

namespace GpsIntersect.Xml
{
    public abstract class ReaderBase
    {
        /// <summary>
        /// Deserializes an XML file.
        /// </summary>
        protected T DeserializeFile<T>(string filename)
        {
            var ser = new XmlSerializer(typeof(T));
            using (XmlReader reader = XmlReader.Create(filename))
                return (T)ser.Deserialize(reader);
        }
    }
}
