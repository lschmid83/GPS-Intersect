
using GpsIntersect.Maths;
using GpsIntersect.Tests.IntegrationTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace GpsIntersect.Tests.UnitTests.Maths
{
    [TestClass]
    public class PolygonTests : TestsBase
    {
        [TestMethod]
        public void CanConstructPolygonWithCoordinates()
        {
            // Arange.
            var polygon = new Polygon(PolygonName, CreateCoordinateCollection(), null);

            // Act => Assert.
            Assert.AreEqual(polygon.Name, PolygonName);
            AssertCoordinateCollection(polygon.Coordinates, CreateCoordinateCollection());
            Assert.AreEqual(polygon.Intersections, null);
        }

        [TestMethod]
        public void CanConstructPolygonWithIntersections()
        {
            // Arrange.
            var polygon = new Polygon(PolygonName, null, CreateCoordinateCollection());

            // Act => Assert.
            Assert.AreEqual(polygon.Name, PolygonName);
            AssertCoordinateCollection(polygon.Intersections, CreateCoordinateCollection());
            Assert.AreEqual(polygon.Coordinates, null);
        }

        [TestMethod]
        public void CanCheckCoordinateInPolygon()
        {
            // Arrange.          
            var coordinates = new ObservableCollection<Coordinate>()
            {
                new Coordinate() { Lat = 1, Lon = 1 },
                new Coordinate() { Lat = 1, Lon = 3 },
                new Coordinate() { Lat = 3, Lon = 3 },
                new Coordinate() { Lat = 3, Lon = 1 },
            };
            var polygon = new Polygon(PolygonName, coordinates, new ObservableCollection<Coordinate>());

            // Act => Assert.
            Assert.IsTrue(polygon.IsInPolygon(new Coordinate() { Lat = 2, Lon = 2 }));
            Assert.IsTrue(polygon.IsInPolygon(new Coordinate() { Lat = 1, Lon = 2 }));
            Assert.IsFalse(polygon.IsInPolygon(new Coordinate() { Lat = 0, Lon = 2 }));
        }
    }
}
