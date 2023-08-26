using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GpsIntersect.Dialogs;
using GpsIntersect.Maths;
using GpsIntersect.Models;
using GpsIntersect.Properties;
using GpsIntersect.Xml;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Application = System.Windows.Application;
using Point = System.Windows.Point;

namespace GpsIntersect.ViewModels
{
    public class MainViewModel : MainlViewModelControl
    {
        public string AppTitle { get { return "GPS Intersect"; } }

        public ObservableCollection<GpsFile> GpsFiles { get; set; } = new ObservableCollection<GpsFile>();

        public GpsFile SelectedGpsFile
        {
            get { return selectedGpsFile; }
            set
            {
                // Set selected file.
                selectedGpsFile = value;
                RaisePropertyChanged("SelectedGpsFile");

                if (selectedGpsFile != null)
                {
                    // Set map position.                    
                    Messenger.Default.Send(new NotificationMessage<GpsFile>(this, selectedGpsFile, "setMapPosition"));
                    
                    // Set selected coordinate.
                    SelectedCoordinate = selectedGpsFile.Coordinates.First();

                    // Update polygon file intersections.
                    if (PolygonFiles.Count > 0)
                    {
                        var polygonFile = GetIntersections(PolygonFiles[0], selectedGpsFile);
                        PolygonFiles.Clear();
                        PolygonFiles.Add(polygonFile);
                    }
                }
            }
        }
        private GpsFile selectedGpsFile;

        public bool CanSaveFile { get; set; }

        public bool CanConvertFile { get; set; }

        public Coordinate SelectedCoordinate
        {
            get { return selectedCoordinate; }
            set
            {
                // Set selected coordinate.
                selectedCoordinate = value;
                RaisePropertyChanged("SelectedCoordinate");
                
                // Set the map marker position.
                if (selectedCoordinate != null && selectedGpsFile != null && selectedGpsFile.Visible)
                    Messenger.Default.Send(new NotificationMessage<Coordinate>(this, selectedCoordinate, "setMarkerPosition"));
            }
        }
        private Coordinate selectedCoordinate;

        public ObservableCollection<PolygonFile> PolygonFiles { get; set; } = new ObservableCollection<PolygonFile>();

        public string BrowserAddress { get; }

        public int OpenedFileCount { get; set; }

        public ICommand NewCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    // Remove files.
                    foreach(var gpsFile in GpsFiles)
                    {
                        // Remove path from map.
                        Messenger.Default.Send(new NotificationMessage<GpsFile>(this, gpsFile, "removeGpsPath"));
                    }

                    // Remove files from collection.
                    GpsFiles.Clear();

                    // Reset open file count.
                    OpenedFileCount = 0;

                    // Remove polygon map.
                    RemovePolygonMap();
                });
            }
        }

        public ICommand OpenFileCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    // Initialize open file dialog.
                    openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    openFileDialog.Filter = "GPX / KML Files(*.gpx; *.kml) | *.gpx; *.kml";
                    openFileDialog.Multiselect = true;

                    var mainWindow = Application.Current?.MainWindow;
                    if (openFileDialog.ShowDialog(mainWindow) == true)
                    {
                        // Remove duplicate filenames.
                        var fileList = openFileDialog.FileNames.ToList();
                        foreach(var file in GpsFiles)
                        {
                            if(openFileDialog.FileNames.Contains(file.Filename))
                                fileList.Remove(file.Filename);
                        }
                        openFileDialog.FileNames = fileList.ToArray();

                        // Read files.
                        GpsFile gpsFile = null;
                        PolygonFile polygonFile = null;
                        foreach (var filename in openFileDialog.FileNames)
                        {
                            if (filename.EndsWith(".gpx"))
                            {
                                try
                                {
                                    gpsFile = gpxReader.ReadGpsFile(filename);
                                }
                                catch (Exception)
                                {
                                    messageBoxDialog.ShowDialog("There was a problem reading the GPX file.\n\n" +
                                                    "Please check the markup is correct.\n\n" +
                                                    "For more information:\n\nhttps://docs.fileformat.com/gis/gpx/", Path.GetFileName(filename),
                                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else if (filename.EndsWith(".kml"))
                            {
                                try
                                {
                                    var fileType = kmlReader.DetectFileType(filename);
                                    if (fileType == KmlReader.KmlFileType.GpsFile)
                                        gpsFile = kmlReader.ReadGpsFile(filename);
                                    else
                                        polygonFile = kmlReader.ReadPolygonFile(filename);
                                }
                                catch (Exception)
                                {
                                    messageBoxDialog.ShowDialog("There was a problem reading the KML file.\n\n" +
                                                    "Please check the markup is correct.\n\n" +
                                                    "For more information:\n\nhttps://developers.google.com/kml/documentation/kml_tut", Path.GetFileName(filename),
                                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }

                            if (gpsFile != null)
                            {
                                // Set file color.
                                gpsFile.Color = GetGpsFileColor();
                                
                                // Add to open file count.
                                OpenedFileCount++;

                                // Add file to collection.
                                GpsFiles.Add(gpsFile);
                                
                                // Draw path on map.
                                Messenger.Default.Send(new NotificationMessage<GpsFile>(this, gpsFile, "addGpsPath"));
                                
                                // Set selected file.
                                SelectedGpsFile = GpsFiles.Last();

                                // File initialized set null.
                                gpsFile = null;
                            }

                            if (polygonFile != null)
                            {
                                // Remove polygon map.
                                RemovePolygonMap();

                                // Add file to collection. (Application only supports 1 polygon file to be loaded at a time).
                                PolygonFiles.Add(polygonFile);

                                // Draw polygons on the map.
                                Messenger.Default.Send(new NotificationMessage<PolygonFile>(this, polygonFile, "addPolygonMap"));

                                // Expand polygon treeview node.
                                PolygonFiles[0].IsExpanded = true;

                                // File initialized set null.
                                polygonFile = null;

                                // Update polygon interestions.
                                SelectedGpsFile = selectedGpsFile;
                            }
                        }

                        // Enable convert file menu option.
                        if (GpsFiles.Count > 0)
                        {
                            CanConvertFile = true;
                            RaisePropertyChanged("CanConvertFile");
                        }

                        if(GpsFiles.Count > 0 && PolygonFiles.Count > 0)
                        {
                            CanSaveFile = true;
                            RaisePropertyChanged("CanSaveFile");
                        }
                    }
                });
            }
        }

        private Color GetGpsFileColor()
        {
            // Initialize file color.
            switch (OpenedFileCount)
            {
                case 0: return Colors.Navy;
                case 1: return Colors.Green;
                case 2: return Colors.Purple;
            }
            var r = new Random();
            return Color.FromRgb((byte)r.Next(0, 255), (byte)r.Next(0, 255), (byte)r.Next(0, 255));
        }

        private PolygonFile GetIntersections(PolygonFile polygonFile, GpsFile gpsFile)
        {
            foreach (var polygon in polygonFile.Polygons)
            {
                // Clear all intersections.
                polygon.Intersections = new ObservableCollection<Coordinate>();
                foreach (var coordinate in gpsFile.Coordinates)
                {
                    // Check for intersection with current coordinate.
                    if (polygon.IsInPolygon(coordinate))
                        polygon.Intersections.Add(coordinate);
                }
            }
            return polygonFile;
        }

        public ICommand IntersectionSelectedCommand
        {
            get
            {
                return new RelayCommand<object>((object coordinate) =>
                {
                    if(coordinate is Coordinate) {

                        // Set selected coordinate.
                        if (coordinate != null && selectedGpsFile != null && selectedGpsFile.Visible)
                            SelectedCoordinate = (Coordinate)coordinate;
                    }
                });
            }
        }

        public ICommand GpsFileCheckedCommand
        {
            get
            {
                return new RelayCommand<GpsFile>((GpsFile gpsFile) =>
                {
                    // Set selected file.
                    SelectedGpsFile = gpsFile;
                    RaisePropertyChanged("SelectedGpsFile");

                    // Set selected coordinate.
                    SelectedCoordinate = selectedGpsFile.Coordinates.First();

                    // Show path visibility.
                    Messenger.Default.Send(new NotificationMessage<GpsFile>(this, selectedGpsFile, "setGpsPathVisibility"));

                    // Set map position. 
                    Messenger.Default.Send(new NotificationMessage<GpsFile>(this, selectedGpsFile, "setMapPosition"));
                });
            }
        }

        public ICommand RemoveGpsFileCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    // Remove path from map.
                    Messenger.Default.Send(new NotificationMessage<GpsFile>(this, selectedGpsFile, "removeGpsPath"));

                    // Remove file from collection.
                    var index = GpsFiles.IndexOf(selectedGpsFile);          
                    GpsFiles.Remove(selectedGpsFile);
                    RaisePropertyChanged("GpsFiles");

                    // Clear polygon intersections.
                    if (PolygonFiles.Count > 0)
                    {
                        var polygonFile = PolygonFiles[0];
                        foreach(var polygon in polygonFile.Polygons)
                            polygon.Intersections = new ObservableCollection<Coordinate>();
                        PolygonFiles.Clear();
                        PolygonFiles.Add(polygonFile);
                    }

                    // Set selected file.
                    if (index > 0)
                    {
                        if (index < GpsFiles.Count)
                            SelectedGpsFile = GpsFiles[index];
                        else
                            SelectedGpsFile = GpsFiles.Last();
                    }
                    else if(GpsFiles.Count > 0)
                        SelectedGpsFile = GpsFiles.First();
                    else
                        SelectedGpsFile = null;

                    // Disable can covert file menu option.
                    if (GpsFiles.Count == 0)
                    {
                        CanConvertFile = false;
                        RaisePropertyChanged("CanConvertFile");
                    }
                });
            }
        }

        public ICommand RemovePolygonFileCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    // Remove polygon map.
                    RemovePolygonMap();

                    // Disable save file menu item.
                    CanSaveFile = false;
                    RaisePropertyChanged("CanSaveFile");
                });
            }
        }

        private void RemovePolygonMap()
        {
            // Remove polygons from map.
            if(PolygonFiles.Count > 0)
                Messenger.Default.Send(new NotificationMessage<PolygonFile>(this, PolygonFiles[0], "removePolygonMap"));

            // Remove file from collection.
            PolygonFiles.Clear();
            RaisePropertyChanged("PolygonFiles");
        }

        public ICommand SaveAsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    // Initialize save file dialog.
                    saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    saveFileDialog.FileName = Path.GetFileNameWithoutExtension(PolygonFiles[0].Filename);
                    saveFileDialog.Filter = "Polygon Intersections File (*.xml)|*.xml";

                    // Show dialog.
                    var mainWindow = Application.Current?.MainWindow;
                    if (saveFileDialog.ShowDialog(mainWindow) == true)
                    {
                        try
                        {
                            // Save polygon intersections to file.
                            intersectionWriter.WriteIntersectionFile(saveFileDialog.FileName, PolygonFiles[0]);
                        }
                        catch(Exception)
                        {
                            messageBoxDialog.ShowDialog("There was a problem saving the file.", Path.GetFileName(saveFileDialog.FileName),
                                            MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                });
            }
        }

        public ICommand CopyCommand
        {
            get
            {
                return new RelayCommand<Grid>((Grid element) =>
                {                   
                    // Get element size.
                    var width = element.Width;
                    var height = element.Height;
                    
                    // Create bitmap target image. 
                    var bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
                    
                    // Draw element to bitmap image.
                    var dv = new DrawingVisual();
                    using (DrawingContext dc = dv.RenderOpen())
                    {
                        VisualBrush vb = new VisualBrush(element);
                        dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
                    }
                    bmpCopied.Render(dv);

                    // Set clipboard image.
                    Clipboard.SetImage(bmpCopied);
                });
            }
        }

        public ICommand ConvertFileCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    // Initialize save file dialog.
                    saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    saveFileDialog.FileName = Path.GetFileNameWithoutExtension(selectedGpsFile.File);
                    var ext = Path.GetExtension(selectedGpsFile.File);
                    if (ext == ".gpx")
                        saveFileDialog.Filter = "KML File (*.kml)|*.kml";
                    else
                        saveFileDialog.Filter = "GPX File (*.gpx)|*.gpx";

                    // Show dialog.
                    var mainWindow = Application.Current?.MainWindow; 
                    if (saveFileDialog.ShowDialog(mainWindow) == true)
                    {
                        // Convert selected file.
                        try
                        {
                            if (ext == ".gpx")
                                kmlWriter.WriteKmlFile(saveFileDialog.FileName, selectedGpsFile);
                            else
                                gpxWriter.WriteGpxFile(saveFileDialog.FileName, selectedGpsFile);
                        }
                        catch(Exception)
                        {
                            messageBoxDialog.ShowDialog("There was a problem converting the file.", Path.GetFileName(saveFileDialog.FileName),
                                            MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                });
            }
        }

        public ICommand AboutCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    messageBoxDialog.ShowDialog("This application displays a GPS route (GPX or KML) on a map and calculates the timestamps of interesections with polygons.\n\n" +
                                    "© 2020 Citex Software - Lawrence Schmid", "About GPS Intersect", MessageBoxButton.OK, MessageBoxImage.None);
                });
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Application.Current.Shutdown();
                });
            }
        }

        private readonly IOpenFileDialog openFileDialog;

        private readonly ISaveFileDialog saveFileDialog;

        private readonly IIMessageBoxDialog messageBoxDialog;

        private readonly IGpxReader gpxReader;

        private readonly IGpxWriter gpxWriter;

        private readonly IKmlReader kmlReader;

        private readonly IKmlWriter kmlWriter;

        private readonly IXmlIntersectionWriter intersectionWriter;

        public MainViewModel(IOpenFileDialog openFileDialog, ISaveFileDialog saveFileDialog, IIMessageBoxDialog messageBoxDialog,
                             IGpxReader gpxReader, IGpxWriter gpxWriter, IKmlReader kmlReader, IKmlWriter kmlWriter, 
                             IXmlIntersectionWriter intersectionWriter)
        {
            // Throw exception if dependencies are not initialized.
            if (openFileDialog == null || saveFileDialog == null || messageBoxDialog == null || 
                gpxReader == null || gpxWriter == null || kmlReader == null || kmlWriter == null || 
                intersectionWriter == null)
                throw new ArgumentNullException();

            // Initialize dependencies.
            this.openFileDialog = openFileDialog;
            this.saveFileDialog = saveFileDialog;
            this.messageBoxDialog = messageBoxDialog;
            this.gpxReader = gpxReader;
            this.gpxWriter = gpxWriter;
            this.kmlReader = kmlReader;
            this.kmlWriter = kmlWriter;
            this.intersectionWriter = intersectionWriter;

            // Initialize control visibility.
            GpsFilesVisibility = Settings.Default.GpsFilesVisibility;
            IntersectionsVisibility = Settings.Default.IntersectionsVisibility;
            TrackPointsVisibility = Settings.Default.TrackPointsVisibility;
            MapLabelsVisibility = Settings.Default.MapLabelsVisibility;
            StatusBarVisibility = Settings.Default.StatusBarVisibility;

            // Initialize menu options enabled.
            CanSaveFile = false;
            CanConvertFile = false;
            
            // Set the location of the map index.html file.
            BrowserAddress = string.Format("file:///{0}/index.html", Directory.GetCurrentDirectory());
        }
    }
}