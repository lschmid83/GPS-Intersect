using GpsIntersect.Maths;
using GpsIntersect.Models;
using GpsIntersect.Xml.Serialization;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Polygon = GpsIntersect.Maths.Polygon;

namespace GpsIntersect.Xml
{
    public class KmlReader : ReaderBase, IKmlReader
    {
        /// <summary>
        /// Reads GpsFile from a KML file.
        /// </summary>
        public GpsFile ReadGpsFile(string filename)
        {
            // Deserialize file.
            var kml = DeserializeFile<Kml>(filename);

            // Read coordinates.
            var coordinates = new ObservableCollection<Coordinate>();
            foreach (var placemark in kml.Document.Folder.FirstOrDefault().Placemark)
            {
                var coordinatesSplit = placemark.Point.Coordinates.Split(',');
                coordinates.Add(new Maths.Coordinate()
                {
                    Lon = double.Parse(coordinatesSplit[0]),
                    Lat = double.Parse(coordinatesSplit[1]),
                    Ele = double.Parse(coordinatesSplit[2]),
                    Time = DateTime.Parse(placemark.TimeStamp.When)
                });
            }

            // Initialize file.
            return new GpsFile(filename, kml.Document.Name, kml.Document.Description, coordinates, true);
        }

        /// <summary>
        /// Read a PolygonFile from a KML file.
        /// </summary>
        public PolygonFile ReadPolygonFile(string filename)
        {
            // Deserialize file.
            var kml = DeserializeFile<Kml>(filename);

            // Read polygons.
            var polygons = new ObservableCollection<Polygon>();
            foreach (var placemark in kml.Document.Folder.FirstOrDefault().Placemark)
            {
                // Split coordinates into lines by whitespace character.
                var coordinatesLines = placemark.Polygon.OuterBoundaryIs.LinearRing.Coordinates.Trim().Split(' ');

                // Add coordinates to polygon file.
                ObservableCollection<Coordinate> coordinates = new ObservableCollection<Coordinate>();
                foreach(var coordinate in coordinatesLines)
                {
                    var coordinateValue = coordinate.Trim().Split(',');
                    coordinates.Add(new Coordinate()
                    {
                        Lon = double.Parse(coordinateValue[0]),
                        Lat = double.Parse(coordinateValue[1]),
                        Ele = double.Parse(coordinateValue[2]),
                    });
                }
                polygons.Add(new Polygon(placemark.Name, coordinates, null));
            }

            // Initialize file.
            return new PolygonFile(filename, kml.Document.Name, kml.Document.Description, polygons);
        }

        // Type of Kml file.
        public enum KmlFileType
        {
            GpsFile = 1,
            PolygonFile = 2
        }

        /// <summary>
        /// Detect if a KML file contains GpsFile or PolygonFile data.
        /// </summary>
        public KmlFileType DetectFileType(string filename)
        {
            // Check for polygon elements.
            var xml = XDocument.Load(filename);
            var polygon = xml.Root.Elements()
                .Where(e => e.Name.LocalName == "Document").Elements()
                .Where(e => e.Name.LocalName == "Folder").Elements()
                .Where(e => e.Name.LocalName == "Placemark").Elements()
                .Where(e => e.Name.LocalName == "Polygon").FirstOrDefault();
            if (polygon != null)
                return KmlFileType.PolygonFile;
            else
                return KmlFileType.GpsFile;
        }
    }
}
