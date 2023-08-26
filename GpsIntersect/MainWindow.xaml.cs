using CefSharp;
using GalaSoft.MvvmLight.Messaging;
using GpsIntersect.Maths;
using GpsIntersect.Models;
using System.Windows;
using System.Windows.Controls;

namespace GpsIntersect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Register CefSharp notifications.
            Messenger.Default.Register<NotificationMessage<GpsFile>>(this, GpsFileNotification);
            Messenger.Default.Register<NotificationMessage<Coordinate>>(this, CoordinateNotification);
            Messenger.Default.Register<NotificationMessage<PolygonFile>>(this, PolygonFileNotification);
        }

        public void GpsFileNotification(NotificationMessage<GpsFile> message)
        {
            // Execute GpsFile JS command.
            Browser.ExecuteScriptAsync(message.Notification + "(" + message.Content.ToJson() + ")");
        }

        public void CoordinateNotification(NotificationMessage<Coordinate> message)
        {
            // Execute Coordinate JS command.
            Browser.ExecuteScriptAsync(message.Notification + "(" + message.Content.ToJson() + ")");
        }

        public void PolygonFileNotification(NotificationMessage<PolygonFile> message)
        {
            // Execute PolygonFile JS Command.
            Browser.ExecuteScriptAsync(message.Notification + "(" + message.Content.ToJson() + ")");
        }

        private void TrackPointListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Scroll ListBox view to item added.
            if(e.AddedItems.Count > 0)
                ((ListBox)sender).ScrollIntoView(e.AddedItems[0]);
        }

        private void Window_Initialized(object sender, System.EventArgs e)
        {
            var window = sender as Window;
            var resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            if (resolution.Width < 1280 || resolution.Height < 960)
            {
                // Set maximized if screen resolution is lower than window size.
                window.WindowState = WindowState.Maximized;
                window.Width = 800;
                window.Height = 600;
            }
        }
    }
}
