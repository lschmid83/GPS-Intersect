using Newtonsoft.Json;
using System;

namespace GpsIntersect.Maths
{
    public class Coordinate
    {
        public double Lat { get; set; }

        public double Lon { get; set; }

        public double Ele { get; set; }

        public DateTime Time { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
