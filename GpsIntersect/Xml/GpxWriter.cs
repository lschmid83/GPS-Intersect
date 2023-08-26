using GpsIntersect.Models;
using GpsIntersect.Xml.Serialization;
using System.Collections.Generic;

namespace GpsIntersect.Xml
{
    public class GpxWriter : WriterBase, IGpxWriter
    {
        /// <summary>
        /// Writes a GpsFile to a GPX file.
        /// </summary>
        public void WriteGpxFile(string filename, GpsFile gpsFile)
        {
            // Create Gpx file.
            var gpxFile = new Gpx
            {
                Trk = new Trk()
                {
                    Name = gpsFile.Name,
                    Desc = gpsFile.Description,
                    Trkseg = new Trkseg()          
                }
            };
            gpxFile.Trk.Trkseg.Trkpt = new List<Trkpt>();

            // Add coordinates to file.
            foreach (var coordinate in gpsFile.Coordinates)
            {
                gpxFile.Trk.Trkseg.Trkpt.Add(new Trkpt()
                {
                    Lat = coordinate.Lat,
                    Lon = coordinate.Lon,
                    Ele = coordinate.Ele,
                    Time = ConvertDateTimeToTimestamp(coordinate.Time)
                });
            }

            // Serialize to file.
            SerializeFile(filename, gpxFile);
        }
    }
}
