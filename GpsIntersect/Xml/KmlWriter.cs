using GpsIntersect.Models;
using GpsIntersect.Xml.Serialization;
using System.Collections.Generic;

namespace GpsIntersect.Xml
{
    public class KmlWriter : WriterBase, IKmlWriter 
    {
        /// <summary>
        /// Writes a GpsFile to a KML file.
        /// </summary>
        public void WriteKmlFile(string filename, GpsFile gpsFile)
        {
            // Create KML file.
            var kmlFile = new Kml
            {
                Document = new Document
                {
                    Name = gpsFile.Name,
                    Description = gpsFile.Description,
                    Folder = new List<Folder>()
                }
            };
            kmlFile.Document.Folder.Add(new Folder());
            kmlFile.Document.Folder[0].Placemark = new List<Placemark>();

            // Add coordinates to file.
            foreach(var coordinate in gpsFile.Coordinates)
            {
                var placemark = new Placemark
                {
                    Point = new Point()
                };
                placemark.Point.Coordinates = coordinate.Lon + "," + coordinate.Lat + "," + coordinate.Ele;
                placemark.TimeStamp = new TimeStamp
                {
                    When = ConvertDateTimeToTimestamp(coordinate.Time)
                };
                kmlFile.Document.Folder[0].Placemark.Add(placemark);
            }

            // Serialize to file.
            SerializeFile(filename, kmlFile);
        }

        /// <summary>
        /// Writes a PolygonFile containing coordinates to a KML file.
        /// </summary>
        public void WritePolygonKmlFile(string filename, PolygonFile polygonFile)
        {
            // Create KML file.
            var kmlFile = new Kml
            {
                Document = new Document
                {
                    Name = polygonFile.Name,
                    Description = polygonFile.Description,
                    Folder = new List<Folder>()
                }
            };
            kmlFile.Document.Folder.Add(new Folder());
            kmlFile.Document.Folder[0].Placemark = new List<Placemark>();

            // Add coordinates to file.
            foreach (var polygon in polygonFile.Polygons)
            {
                var placemark = new Placemark
                {
                    Name = polygon.Name,
                    Polygon = new Polygon()
                };

                placemark.Polygon.OuterBoundaryIs = new OuterBoundaryIs
                {
                    LinearRing = new LinearRing()
                };

                foreach (var coordinate in polygon.Coordinates)
                {
                    placemark.Polygon.OuterBoundaryIs.LinearRing.Coordinates += coordinate.Lon + "," + coordinate.Lat + "," + coordinate.Ele + " ";
                }

                kmlFile.Document.Folder[0].Placemark.Add(placemark);
            }

            // Serialize to file.
            SerializeFile(filename, kmlFile);
        }
    }
}
