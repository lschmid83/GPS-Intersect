using GpsIntersect.Models;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace GpsIntersect.Xml
{
    public class XmlIntersectionWriter : WriterBase, IXmlIntersectionWriter
    {
        /// <summary>
        /// Writes a PolygonFile containing intersections to a XML file.
        /// </summary>
        public void WriteIntersectionFile(string filename, PolygonFile polygonFile)
        {
            // Ignore Polygon->Coordinates property during serialization.
            var ignoreAttribute = new XmlAttributes { XmlIgnore = true };
            var overrideAttribute = new XmlAttributeOverrides();
            overrideAttribute.Add(typeof(Polygon), "Coordinates", ignoreAttribute);

            // Serialize to file.
            SerializeFile(filename, polygonFile, overrideAttribute);
        }
    }
}
