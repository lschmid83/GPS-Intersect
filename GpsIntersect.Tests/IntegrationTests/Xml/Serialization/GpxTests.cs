using GpsIntersect.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace GpsIntersect.Tests.IntegrationTests.Xml.Serialization
{
    [TestClass]
    public class GpxTests : TestsBase
    {
        [TestMethod]
        public void CanReadWriteGpxFile()
        {
            // Arrange.
            var expectedGpxFile = CreateGpsFile(GpxFileName, CreateCoordinateCollection());
            
            // Act.
            new GpxWriter().WriteGpxFile(GpxFileName, expectedGpxFile);                  
            var actualGpsFile = new GpxReader().ReadGpsFile(GpxFileName);

            // Assert.
            AssertGpsFile(expectedGpxFile, actualGpsFile);
            AssertCoordinateCollection(expectedGpxFile.Coordinates, actualGpsFile.Coordinates);
            Assert.AreEqual(GpxFileName, actualGpsFile.Filename);

            // Clean-up.
            File.Delete(GpxFileName);
        }

        [TestMethod]
        public void ThrowsReadInvalidGpxException()
        {
            // Arrange.          
            new GpxWriter().WriteGpxFile(GpxFileName, CreateGpsFile(GpxFileName, CreateCoordinateCollection()));
            File.WriteAllLines(GpxFileName, File.ReadAllLines(GpxFileName).Skip(4).ToArray()); // Remove first 4 lines of file.

            // Act => Assert
            Assert.ThrowsException<InvalidOperationException>(() => new GpxReader().ReadGpsFile(GpxFileName));

            // Clean-up.
            File.Delete(GpxFileName);
        }
    }
}
