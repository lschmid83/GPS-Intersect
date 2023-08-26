using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GpsIntersect.Xml.Serialization
{
    /// <summary>
    /// Serializable class for GPX file format.
    /// See: https://docs.fileformat.com/gis/gpx/
    /// </summary>
    [Serializable()]
    [XmlRoot("gpx", Namespace = "http://www.topografix.com/GPX/1/0")]
    public class Gpx
    {
        [XmlElement("trk")]
        public Trk Trk { get; set; }
    }

    public partial class Trk
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("desc")]
        public string Desc { get; set; }

        [XmlElement("trkseg")]
        public Trkseg Trkseg { get; set; }
    }

    public partial class Trkseg
    {
        [XmlElement("trkpt")]
        public List<Trkpt> Trkpt { get; set; }
    }

    public partial class Trkpt
    {
        [XmlAttribute("lat")]
        public double Lat { get; set; }

        [XmlAttribute("lon")]
        public double Lon { get; set; }

        [XmlElement("ele")]
        public double Ele { get; set; }

        [XmlElement("time")]
        public string Time { get; set; }
    }
}
