using System.Collections.ObjectModel;
using System.Linq;

namespace GpsIntersect.Maths
{
    public class Polygon
    {
        public string Name { get; set; }

        public ObservableCollection<Coordinate> Coordinates { get; set; }

        public ObservableCollection<Coordinate> Intersections { get; set; }

        public Polygon() { }

        public Polygon(string name, ObservableCollection<Coordinate> coordinates, ObservableCollection<Coordinate> intersections)
        {
            Name = name;
            Coordinates = coordinates;
            Intersections = intersections;
        }

        /// <summary>
        /// Determines if the given coordinate is inside a polygon.
        /// </summary>
        public bool IsInPolygon(Coordinate coordinate)
        {
            // Adapted from: https://stackoverflow.com/questions/4243042/c-sharp-point-in-polygon
            var coef = Coordinates.Skip(1).Select((p, i) =>
                                            (coordinate.Lon - Coordinates[i].Lon) * (p.Lat - Coordinates[i].Lat) - 
                                            (coordinate.Lat - Coordinates[i].Lat) * (p.Lon - Coordinates[i].Lon))
                                          .ToList();

            if (coef.Any(p => p == 0))
                return true;

            for (int i = 1; i < coef.Count(); i++)
            {
                if (coef[i] * coef[i - 1] < 0)
                    return false;
            }
            return true;
        }
    }
}
