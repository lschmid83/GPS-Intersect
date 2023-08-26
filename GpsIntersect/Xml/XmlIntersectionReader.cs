using GpsIntersect.Models;

namespace GpsIntersect.Xml
{
    public class XmlIntersectionReader : ReaderBase, IXmlIntersectionReader
    {
        /// <summary>
        /// Reads a PolygonFile containing intersections from a XML file.
        /// </summary>
        public PolygonFile ReadIntersectionsFile(string filename)
        {
            var polygonFile = DeserializeFile<PolygonFile>(filename);
            polygonFile.Filename = filename;
            foreach(var polygon in polygonFile.Polygons)
                polygon.Coordinates = null;
            return polygonFile;
        }
    }
}
