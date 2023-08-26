using GpsIntersect.Models;
using GpsIntersect.Tests.IntegrationTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GpsIntersect.Tests.UnitTests.Models
{
    [TestClass]
    public class PolygonFileTests : TestsBase
    {
        [TestMethod]
        public void CanConstructPolygonFile()
        {
            // Arrange.
            var polygonFile = new PolygonFile(PolygonFileName, PolygonName, PolygonDescription, CreatePolygonCollection(false));

            // Act => Assert.
            Assert.AreEqual(polygonFile.Filename, PolygonFileName);
            Assert.AreEqual(polygonFile.Name, PolygonName);
            Assert.AreEqual(polygonFile.Description, PolygonDescription);
            Assert.AreEqual(polygonFile.IsExpanded, false);
            AssertPolygonCollection(polygonFile.Polygons, CreatePolygonCollection(false), false);
        }

        [TestMethod]
        public void CanConstructPolygonIntersectionFile()
        {
            // Arrange.
            var polygonIntersectionFile = new PolygonFile(PolygonFileName, PolygonName, PolygonDescription, CreatePolygonCollection(true));

            // Act => Assert.
            Assert.AreEqual(polygonIntersectionFile.Filename, PolygonFileName);
            Assert.AreEqual(polygonIntersectionFile.Name, PolygonName);
            Assert.AreEqual(polygonIntersectionFile.Description, PolygonDescription);
            Assert.AreEqual(polygonIntersectionFile.IsExpanded, false);
            AssertPolygonCollection(polygonIntersectionFile.Polygons, CreatePolygonCollection(true), true);
        }
    }
}
