using GpsIntersect.Dialogs;
using GpsIntersect.Maths;
using GpsIntersect.Models;
using GpsIntersect.ViewModels;
using GpsIntersect.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace GpsIntersect.Tests.IntegrationTests
{
    public class TestsBase
    {
        public string GpxFileName = Directory.GetCurrentDirectory() + @"\test-gps.gpx";
        public string KmlFileName = Directory.GetCurrentDirectory() + @"\test-kml.kml";
        public string GpsName = "GPS File";
        public string GpsDescription = "GPS File Description";

        public string PolygonFileName = Directory.GetCurrentDirectory() + @"\test-polygon.kml";
        public string PolygonName = "Polygon File";
        public string PolygonDescription = "Polygon File Description";

        public string IntersectionFileName = Directory.GetCurrentDirectory() + @"\test-intersection.xml";

        public GpsFile CreateGpsFile(string filename, ObservableCollection<Coordinate> coordinates)
        {
            return new GpsFile(filename, GpsName, GpsDescription, coordinates, true);
        }

        public void AssertGpsFile(GpsFile expected, GpsFile actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Visible, actual.Visible);
        }

        public ObservableCollection<Coordinate> CreateCoordinateCollection()
        {
            var coordinates = new ObservableCollection<Coordinate>();
            for (var i = 0; i < 10; i++)
            {
                coordinates.Add(new Coordinate()
                {
                    Lat = i,
                    Lon = i + 1,
                    Ele = i + 2,
                    Time = new DateTime(2020, 1, 1, 1, 1, 1, 1).AddSeconds(i)
                });
            }
            return coordinates;
        }

        public void AssertCoordinateCollection(ObservableCollection<Coordinate> expected, ObservableCollection<Coordinate> actual)
        {
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(expected[i].Lat, actual[i].Lat);
                Assert.AreEqual(expected[i].Lon, actual[i].Lon);
                Assert.AreEqual(expected[i].Ele, actual[i].Ele);
                Assert.AreEqual(expected[i].Time.ToString(), actual[i].Time.ToString());
            }
        }

        public PolygonFile CreatePolygonFile(string filename, ObservableCollection<Polygon> polygons)
        {
            return new PolygonFile(filename, PolygonName, PolygonDescription, polygons);
        }

        public void AssertPolygonFile(PolygonFile expected, PolygonFile actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
        }

        public ObservableCollection<Polygon> CreatePolygonCollection(bool intersections)
        {
            var polygons = new ObservableCollection<Polygon>();
            for (var i = 0; i < 10; i++)
            {
                if (intersections)
                    polygons.Add(new Polygon("Polygon " + i, null, CreateCoordinateCollection()));
                else
                    polygons.Add(new Polygon("Polygon " + i, CreateCoordinateCollection(), null));    
            }
            return polygons;
        }

        public void AssertPolygonCollection(ObservableCollection<Polygon> expected, ObservableCollection<Polygon> actual, bool intersections)
        {
            for (var a = 0; a < 10; a++)
            {
                for (var b = 0; b < 10; b++)
                {
                    Coordinate expectedCoordinate;
                    Coordinate actualCoordinate;
                    if (intersections)
                    {
                        expectedCoordinate = expected[a].Intersections[b];
                        actualCoordinate = actual[a].Intersections[b];

                        Assert.AreEqual(expected[a].Coordinates, null);
                        Assert.AreEqual(actual[a].Coordinates, null);
                    }
                    else
                    {
                        expectedCoordinate = expected[a].Coordinates[b];
                        actualCoordinate = actual[a].Coordinates[b];

                        Assert.AreEqual(expected[a].Intersections, null);
                        Assert.AreEqual(actual[a].Intersections, null);
                    }
                    Assert.AreEqual(expectedCoordinate.Lat, actualCoordinate.Lat);
                    Assert.AreEqual(expectedCoordinate.Lon, actualCoordinate.Lon);
                    Assert.AreEqual(expectedCoordinate.Ele, actualCoordinate.Ele);

                    if (intersections)
                        Assert.AreEqual(expectedCoordinate.Time.ToString(), actualCoordinate.Time.ToString());
                }
            }
        } 
        
        public MainViewModel CreateMainViewModel()
        {
            return new MainViewModel(new OpenFileDialog(), new SaveFileDialog(), new MessageBoxDialog(),
                new GpxReader(), new GpxWriter(), new KmlReader(), new KmlWriter(), new XmlIntersectionWriter());
        }
    }
}
