using GpsIntersect.Dialogs;
using GpsIntersect.Maths;
using GpsIntersect.ViewModels;
using GpsIntersect.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GpsIntersect.Tests.IntegrationTests.ViewModels
{
    [TestClass]
    public class MainViewModelTests : TestsBase
    {
        [TestMethod]
        public void CanConstructMainViewModel()
        {
            // Arrange.
            var mainViewModel = CreateMainViewModel();

            // Act => Assert.
            Assert.AreEqual(false, mainViewModel.CanSaveFile);
            Assert.AreEqual(false, mainViewModel.CanConvertFile);
            Assert.AreEqual(string.Format("file:///{0}/index.html", Directory.GetCurrentDirectory()), mainViewModel.BrowserAddress);
        }

        [TestMethod]
        public void CanSetSelectedGpsFile()
        {
            // Arrange.
            var mainViewModel = CreateMainViewModel();
            var expectedGpxFile = CreateGpsFile(GpxFileName, CreateCoordinateCollection());
            mainViewModel.GpsFiles.Add(expectedGpxFile);

            // Act.
            mainViewModel.SelectedGpsFile = expectedGpxFile;

            // Assert.
            Assert.AreEqual(expectedGpxFile, mainViewModel.SelectedGpsFile);
            Assert.AreEqual(expectedGpxFile.Coordinates.First(), mainViewModel.SelectedCoordinate);
        }

        [TestMethod]
        public void CanInvokeNewFileCommand() {
            
            // Arrange.
            var mainViewModel = CreateMainViewModel();
            mainViewModel.NewCommand.Execute(null);

            // Act => Assert.
            Assert.AreEqual(0, mainViewModel.GpsFiles.Count);
            Assert.AreEqual(0, mainViewModel.OpenedFileCount);
            Assert.AreEqual(0, mainViewModel.PolygonFiles.Count);
        }

        [TestMethod]
        public void CanInvokeOpenFileCommand()
        {
            // Arrange.

            // Create GPX File.
            var expectedGpxFile = CreateGpsFile(GpxFileName, CreateCoordinateCollection());
            new GpxWriter().WriteGpxFile(GpxFileName, expectedGpxFile);
            
            // Create KML File.
            var expectedKmlFile = CreateGpsFile(KmlFileName, CreateCoordinateCollection());
            new KmlWriter().WriteKmlFile(KmlFileName, expectedKmlFile);

            // Create PolygonFile.
            var expectedPolygonFile = CreatePolygonFile(PolygonFileName, CreatePolygonCollection(false));
            new KmlWriter().WritePolygonKmlFile(PolygonFileName, expectedPolygonFile);

            // Mock open file dialog.
            var mockOpenFileDialog = new Mock<IOpenFileDialog>();
            mockOpenFileDialog.Setup(x => x.ShowDialog(null)).Returns(true);
            mockOpenFileDialog.Setup(x => x.FileNames).Returns(new string[]
            {
                GpxFileName,
                KmlFileName,
                PolygonFileName
            });

            // Mock messsge box dialog.
            var mockMessageBoxDialog = new Mock<IIMessageBoxDialog>();
            mockMessageBoxDialog.Setup(x => x.ShowDialog(null, null, MessageBoxButton.OK, MessageBoxImage.Error));

            // Construct MainViewModel with dependencies.
            var mainViewModel = new MainViewModel(mockOpenFileDialog.Object, new SaveFileDialog(), mockMessageBoxDialog.Object,
                new GpxReader(), new GpxWriter(), new KmlReader(), new KmlWriter(), new XmlIntersectionWriter());

            // Act.
            mainViewModel.OpenFileCommand.Execute(null);

            // Assert.
            Assert.AreEqual(2, mainViewModel.GpsFiles.Count);
            Assert.AreEqual(2, mainViewModel.OpenedFileCount);
            AssertGpsFile(expectedGpxFile, mainViewModel.GpsFiles[0]);
            AssertGpsFile(expectedKmlFile, mainViewModel.GpsFiles[1]);
            Assert.AreEqual(1, mainViewModel.PolygonFiles.Count);
            AssertPolygonFile(expectedPolygonFile, mainViewModel.PolygonFiles[0]);
            Assert.AreEqual(true, mainViewModel.CanConvertFile);
            Assert.AreEqual(true, mainViewModel.CanSaveFile);

            // Clean-up.
            File.Delete(GpxFileName);
            File.Delete(KmlFileName);
            File.Delete(PolygonFileName);
        }

        [TestMethod]
        public void CanInvokeIntersectionSelectedCommand()
        {
            // Arrange.
            var mainViewModel = CreateMainViewModel();
            var expectedGpxFile = CreateGpsFile(GpxFileName, CreateCoordinateCollection());
            mainViewModel.SelectedGpsFile = expectedGpxFile;
            var expectedCoordinate = new Coordinate() { Lat = 0, Lon = 1, Ele = 3, Time = DateTime.Now };
            
            // Act.
            mainViewModel.IntersectionSelectedCommand.Execute(expectedCoordinate);

            // Assert.
            Assert.AreEqual(expectedGpxFile, mainViewModel.SelectedGpsFile);
            Assert.AreEqual(expectedCoordinate, mainViewModel.SelectedCoordinate);
        }

        [TestMethod]
        public void CanInvokeGpsFileCheckedCommand()
        {
            // Arrange.
            var mainViewModel = CreateMainViewModel();
            var gpsFile = CreateGpsFile(GpxFileName, CreateCoordinateCollection());

            // Act.
            mainViewModel.GpsFileCheckedCommand.Execute(gpsFile);

            // Assert.
            Assert.AreEqual(gpsFile, mainViewModel.SelectedGpsFile);
            Assert.AreEqual(gpsFile.Coordinates.First(), mainViewModel.SelectedCoordinate);
        }

        [TestMethod]
        public void CanInvokeRemoveGpsFileCommand() 
        {
            // Arrange.
            var mainViewModel = CreateMainViewModel();
            var expectedGpxFile = CreateGpsFile(GpxFileName, CreateCoordinateCollection());
            mainViewModel.GpsFiles.Add(expectedGpxFile);
            mainViewModel.SelectedGpsFile = expectedGpxFile;

            // Act.
            mainViewModel.RemoveGpsFileCommand.Execute(expectedGpxFile);

            // Assert.
            Assert.AreEqual(null, mainViewModel.SelectedGpsFile);
            Assert.AreEqual(false, mainViewModel.CanConvertFile);
            Assert.AreEqual(0, mainViewModel.GpsFiles.Count);
        }

        [TestMethod]
        public void CanInvokeRemovePolygonFileCommand()
        {
            // Arrange.
            var mainViewModel = CreateMainViewModel();
            var expectedPolygonFile = CreatePolygonFile(PolygonFileName, CreatePolygonCollection(false));
            mainViewModel.PolygonFiles.Add(expectedPolygonFile);

            // Act.
            mainViewModel.RemovePolygonFileCommand.Execute(null);

            // Assert.
            Assert.AreEqual(0, mainViewModel.PolygonFiles.Count);
            Assert.AreEqual(false, mainViewModel.CanSaveFile);
        }

        [TestMethod]
        public void CanInvokeSaveAsCommand()
        {
            // Arrange.
            var expectedIntersectionFile = CreatePolygonFile(IntersectionFileName, CreatePolygonCollection(true));

            // Mock save file dialog.
            var mockSaveFileDialog = new Mock<ISaveFileDialog>();
            mockSaveFileDialog.Setup(x => x.ShowDialog(null)).Returns(true);
            mockSaveFileDialog.Setup(x => x.FileName).Returns(IntersectionFileName);

            // Mock messsge box dialog;
            var mockMessageBoxDialog = new Mock<IIMessageBoxDialog>();
            mockMessageBoxDialog.Setup(x => x.ShowDialog(null, null, MessageBoxButton.OK, MessageBoxImage.Error));

            // Construct MainViewModel with dependencies.
            var mainViewModel = new MainViewModel(new OpenFileDialog(), mockSaveFileDialog.Object, mockMessageBoxDialog.Object,
                new GpxReader(), new GpxWriter(), new KmlReader(), new KmlWriter(), new XmlIntersectionWriter());
            mainViewModel.PolygonFiles.Add(expectedIntersectionFile);

            // Act.
            mainViewModel.SaveAsCommand.Execute(null);

            // Assert
            Assert.AreEqual(true, File.Exists(IntersectionFileName));
            AssertPolygonFile(expectedIntersectionFile, mainViewModel.PolygonFiles[0]);
            AssertPolygonCollection(expectedIntersectionFile.Polygons, mainViewModel.PolygonFiles[0].Polygons, true);
            Assert.AreEqual(IntersectionFileName, mainViewModel.PolygonFiles[0].Filename);

            // Clean-up.
            File.Delete(Path.GetFileNameWithoutExtension(IntersectionFileName) + ".xml");
        }

        [TestMethod]
        public void CanInvokeCopyCommand()
        {
            // Arrange.
            var mainViewModel = CreateMainViewModel();
            var expectedGrid = new Grid { Width = 800, Height = 600 };

            // Act.
            mainViewModel.CopyCommand.Execute(expectedGrid);

            // Assert.
            var actualClipboardImage = Clipboard.GetImage();
            Assert.IsNotNull(actualClipboardImage);
            Assert.AreEqual(expectedGrid.Width, actualClipboardImage.Width);
            Assert.AreEqual(expectedGrid.Height, actualClipboardImage.Height);
        }

        [TestMethod]
        public void CanInvokeConvertFileCommand()
        {
            // Arrange.
           
            // Create GPX File.
            var expectedGpxFile = CreateGpsFile(GpxFileName, CreateCoordinateCollection());
            new GpxWriter().WriteGpxFile(GpxFileName, expectedGpxFile);
            
            // Mock save file dialog.
            var mockSaveFileDialog = new Mock<ISaveFileDialog>();
            mockSaveFileDialog.Setup(x => x.ShowDialog(null)).Returns(true);
            mockSaveFileDialog.Setup(x => x.FileName).Returns(KmlFileName);

            // Mock messsge box dialog.
            var mockMessageBoxDialog = new Mock<IIMessageBoxDialog>();
            mockMessageBoxDialog.Setup(x => x.ShowDialog(null, null, MessageBoxButton.OK, MessageBoxImage.Error));

            // Construct MainViewModel with dependencies.
            var mainViewModel = new MainViewModel(new OpenFileDialog(), mockSaveFileDialog.Object, mockMessageBoxDialog.Object,
                new GpxReader(), new GpxWriter(), new KmlReader(), new KmlWriter(), new XmlIntersectionWriter())
            {
                SelectedGpsFile = expectedGpxFile
            };

            // Act.
            mainViewModel.ConvertFileCommand.Execute(null);

            // Assert.
            File.Exists(KmlFileName);
            var actualKmlFile = new KmlReader().ReadGpsFile(KmlFileName);
            AssertGpsFile(expectedGpxFile, actualKmlFile);
            AssertCoordinateCollection(expectedGpxFile.Coordinates, actualKmlFile.Coordinates);
            Assert.AreEqual(KmlFileName, actualKmlFile.Filename);
        }
    }
}
