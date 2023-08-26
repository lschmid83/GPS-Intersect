using System.Windows;

namespace GpsIntersect.Dialogs
{
    public interface IOpenFileDialog
    {
        string FileName { get; set; }

        string[] FileNames { get; set; }

        string InitialDirectory { get; set; }

        string Filter { get; set; }

        string DefaultExt { get; set; }
        
        bool Multiselect { get; set; }

        bool? ShowDialog(Window owner);
    }
}
