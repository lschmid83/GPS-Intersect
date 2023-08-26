using GpsIntersect.Maths;
using GpsIntersect.Models;
using GpsIntersect.Xml.Serialization;
using System;
using System.Collections.ObjectModel;

namespace GpsIntersect.Xml
{
    public class GpxReader : ReaderBase, IGpxReader
    {
        /// <summary>
        /// Reads a GpsFile from a GPX file.
        /// </summary>
        public GpsFile ReadGpsFile(string filename)
        {
            // Deserialize file.
            var gpx = DeserializeFile<Gpx>(filename);

            // Read coordinates.
            var coordinates = new ObservableCollection<Coordinate>();
            foreach (var trkpt in gpx.Trk.Trkseg.Trkpt)
            {
                coordinates.Add(new Maths.Coordinate()
                {
                    Lat = trkpt.Lat,
                    Lon = trkpt.Lon,
                    Ele = trkpt.Ele,
                    Time = DateTime.Parse(trkpt.Time)
                });
            }

            // Initialize file.
            return new GpsFile(filename, gpx.Trk.Name, gpx.Trk.Desc, coordinates, true);
        }
    }
}
