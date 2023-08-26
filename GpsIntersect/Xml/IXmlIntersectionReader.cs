using GpsIntersect.Models;

namespace GpsIntersect.Xml
{
    public interface IXmlIntersectionReader
    {
        PolygonFile ReadIntersectionsFile(string filename);
    }
}
