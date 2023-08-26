using GpsIntersect.Maths;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace GpsIntersect.Models
{
    public class GpsFile
    {
        public string Filename { get; set; }

        public string File 
        { 
            get
            {
                return Path.GetFileName(Filename);
            }
        }

        public bool Visible { get; set; } = true;

        public string Name { get; set; }

        public string Description { get; set; }

        public Color Color { get; set; }

        public SolidColorBrush ColorBrush
        {
            get 
            { 
                return new SolidColorBrush(Color); 
            }
        }

        public string ColorRgb 
        { 
            get
            {
                return "#" + Color.ToString().Substring(3);
            } 
        }

        public ObservableCollection<Coordinate> Coordinates { get; set; }

        public GpsFile(string filename, string name, string description, ObservableCollection<Coordinate> coordinates, bool visible)
        {
            Filename = filename;
            Name = name;
            Description = description;
            Visible = visible;
            Coordinates = coordinates;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
