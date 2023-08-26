using GpsIntersect.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using static GpsIntersect.Xml.KmlReader;

namespace GpsIntersect.Tests.IntegrationTests.Xml.Serialization
{
    [TestClass]
    public class KmlTests : TestsBase
    {
        [TestMethod]
        public void CanReadWriteKmlFile()
        {
            // Arrange.
            var expectedKmlFile = CreateGpsFile(KmlFileName, CreateCoordinateCollection());

            // Act.
            new KmlWriter().WriteKmlFile(KmlFileName, expectedKmlFile);
            var actualKmlFile = new KmlReader().ReadGpsFile(KmlFileName);

            // Assert.
            AssertGpsFile(expectedKmlFile, actualKmlFile);
            AssertCoordinateCollection(expectedKmlFile.Coordinates, actualKmlFile.Coordinates);
            Assert.AreEqual(KmlFileName, actualKmlFile.Filename);

            // Clean-up.
            File.Delete(GpxFileName);
        }

        [TestMethod]
        public void CanReadWritePolygonKmlFile()
        {
            // Arrange.
            var expectedPolygonFile = CreatePolygonFile(PolygonFileName, CreatePolygonCollection(false));

            // Act.
            new KmlWriter().WritePolygonKmlFile(PolygonFileName, expectedPolygonFile);
            var actualPolygonFile = new KmlReader().ReadPolygonFile(PolygonFileName);

            // Assert.
            AssertPolygonFile(expectedPolygonFile, actualPolygonFile);
            AssertPolygonCollection(expectedPolygonFile.Polygons, actualPolygonFile.Polygons, false);
            Assert.AreEqual(PolygonFileName, actualPolygonFile.Filename);

            // Clean-up.
            File.Delete(PolygonFileName);
        }

        [TestMethod]
        public void CanDetectKmlFileType()
        {
            // Arrange.
            var expectedKmlFile = CreateGpsFile(KmlFileName, CreateCoordinateCollection());
            new KmlWriter().WriteKmlFile(KmlFileName, expectedKmlFile);
            var expectedPolygonFile = CreatePolygonFile(PolygonFileName, CreatePolygonCollection(false));
            new KmlWriter().WritePolygonKmlFile(PolygonFileName, expectedPolygonFile);

            // Act.
            var expectedKmlFileType = new KmlReader().DetectFileType(KmlFileName);
            var expectedPolygonFileType = new KmlReader().DetectFileType(PolygonFileName);

            // Assert.
            Assert.AreEqual(expectedKmlFileType, KmlFileType.GpsFile);
            Assert.AreEqual(expectedPolygonFileType, KmlFileType.PolygonFile);

            // Clean-up.
            File.Delete(GpxFileName);
            File.Delete(PolygonFileName);
        }

        [TestMethod]
        public void ThrowsReadInvalidKmlException()
        {
            // Arrange.          
            new KmlWriter().WriteKmlFile(GpxFileName, CreateGpsFile(GpxFileName, CreateCoordinateCollection()));
            File.WriteAllLines(GpxFileName, File.ReadAllLines(GpxFileName).Skip(4).ToArray()); // Remove first 4 lines of file.

            // Act => Assert.
            Assert.ThrowsException<InvalidOperationException>(() => new GpxReader().ReadGpsFile(GpxFileName));

            // Clean-up.
            File.Delete(GpxFileName);
        }
    }
}