using GpsIntersect.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace GpsIntersect.Tests.IntegrationTests.Xml.Serialization
{
    [TestClass]
    public class XmlTests : TestsBase
    {
        [TestMethod]
        public void CanReadWriteIntersectionsKmlFile()
        {
            // Arrange.
            var expectedIntersectionsFile = CreatePolygonFile(IntersectionFileName, CreatePolygonCollection(true));

            // Act.
            new XmlIntersectionWriter().WriteIntersectionFile(IntersectionFileName, expectedIntersectionsFile);
            var actualIntersectionsFile = new XmlIntersectionReader().ReadIntersectionsFile(IntersectionFileName);

            // Assert.
            AssertPolygonFile(expectedIntersectionsFile, actualIntersectionsFile);
            AssertPolygonCollection(expectedIntersectionsFile.Polygons, actualIntersectionsFile.Polygons, true);
            Assert.AreEqual(IntersectionFileName, actualIntersectionsFile.Filename);

            // Clean-up.
            File.Delete(IntersectionFileName);
        }
    }
}
