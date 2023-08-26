using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GpsIntersect.Xml.Serialization
{
    /// <summary>
    /// Serializable class for KML file format.
    /// See: https://developers.google.com/kml/documentation/kml_tut
    /// </summary>
    [Serializable()]
    [XmlRoot("kml", Namespace = "http://www.opengis.net/kml/2.2")]
    public class Kml
    {
        [XmlElement("Document")]
        public Document Document { get; set; }
    }

    public partial class Document
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("Folder")]
        public List<Folder> Folder { get; set; }
    }

    public partial class Folder
    {
        [XmlElement("Placemark")]
        public List<Placemark> Placemark { get; set; }
    }

    public partial class Placemark
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("TimeStamp")]
        public TimeStamp TimeStamp { get; set; }

        [XmlElement("Point")]
        public Point Point { get; set; }

        [XmlElement("Polygon")]
        public Polygon Polygon { get; set; }
    }

    public partial class TimeStamp
    {
        [XmlElement("when")]
        public string When { get; set; }
    }

    public partial class Point
    {
        [XmlElement("coordinates")]
        public string Coordinates { get; set; }
    }

    public partial class Polygon
    {
        [XmlElement("outerBoundaryIs")]
        public OuterBoundaryIs OuterBoundaryIs { get; set; }
    }

    public partial class OuterBoundaryIs
    {
        [XmlElement("LinearRing")]
        public LinearRing LinearRing { get; set; }
    }

    public partial class LinearRing
    {
        [XmlElement("coordinates")]
        public string Coordinates { get; set; }
    }
}
