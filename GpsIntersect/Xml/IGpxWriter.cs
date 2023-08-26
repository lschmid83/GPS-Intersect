using GpsIntersect.Models;

namespace GpsIntersect.Xml
{
    public interface IGpxWriter
    {
        void WriteGpxFile(string filename, GpsFile gpsFile);
    }
}
