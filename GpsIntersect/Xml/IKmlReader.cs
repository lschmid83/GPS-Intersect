using GpsIntersect.Models;
using static GpsIntersect.Xml.KmlReader;

namespace GpsIntersect.Xml
{
    public interface IKmlReader
    {
        GpsFile ReadGpsFile(string filename);
        PolygonFile ReadPolygonFile(string filename);
        KmlFileType DetectFileType(string filename);
    }
}