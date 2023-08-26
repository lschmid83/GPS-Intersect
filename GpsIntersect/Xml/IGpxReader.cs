using GpsIntersect.Models;

namespace GpsIntersect.Xml
{
    public interface IGpxReader
    {
        GpsFile ReadGpsFile(string filename);
    }
}