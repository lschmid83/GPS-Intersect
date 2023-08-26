using GpsIntersect.Models;

namespace GpsIntersect.Xml
{
    public interface IXmlIntersectionWriter
    {
        void WriteIntersectionFile(string filename, PolygonFile polygonFile);
    }
}
