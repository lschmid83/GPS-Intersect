using GpsIntersect.Models;
using System.Collections.Generic;

namespace GpsIntersect.Xml
{
    public interface IGpsFileReader
    {
        GpsFile ReadGpsFile(string filename);
        PolygonFile ReadPolygonFile(string filename);
    }
}