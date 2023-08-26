using System.Windows;

namespace GpsIntersect.Dialogs
{
    public interface ISaveFileDialog
    {
        string FileName { get; set; }

        string[] FileNames { get; set; }

        string InitialDirectory { get; set; }

        string Filter { get; set; }

        string DefaultExt { get; set; }

        bool? ShowDialog(Window owner);
    }
}
