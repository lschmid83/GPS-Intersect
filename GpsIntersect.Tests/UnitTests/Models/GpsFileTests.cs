using GpsIntersect.Models;
using GpsIntersect.Tests.IntegrationTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Windows.Media;

namespace GpsIntersect.Tests.UnitTests.Models
{
    [TestClass]
    public class GpsFileTests : TestsBase
    {
        [TestMethod]
        public void CanConstructGpsFile()
        {
            // Arrange.
            var gpsFile = new GpsFile(GpxFileName, GpsName, GpsDescription, CreateCoordinateCollection(), false);

            // Act => Assert.
            Assert.AreEqual(gpsFile.Filename, GpxFileName);
            Assert.AreEqual(gpsFile.Name, GpsName);
            Assert.AreEqual(gpsFile.Description, GpsDescription);
            AssertCoordinateCollection(gpsFile.Coordinates, CreateCoordinateCollection());
            Assert.AreEqual(gpsFile.Visible, false);
        }

        [TestMethod]
        public void CanSetGpsFileColor()
        {
            // Arrange.
            var gpsFile = CreateGpsFile(GpxFileName, CreateCoordinateCollection());
            gpsFile.Color = Colors.Navy;

            // Act => Assert.
            Assert.AreEqual(gpsFile.Color, Colors.Navy);
            Assert.AreEqual(gpsFile.ColorBrush.Color, Colors.Navy);
            Assert.AreEqual(gpsFile.ColorRgb, "#" + gpsFile.Color.ToString().Substring(3));
        }
    }
}
