using GpsIntersect.Models;

namespace GpsIntersect.Xml
{
    public interface IKmlWriter
    {
        void WriteKmlFile(string filename, GpsFile gpsFile);
        void WritePolygonKmlFile(string filename, PolygonFile polygonFile);
    }
}
