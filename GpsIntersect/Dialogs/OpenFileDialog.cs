using System.Windows;

namespace GpsIntersect.Dialogs
{
    public class OpenFileDialog : IOpenFileDialog
    {
        public string FileName { get; set; }

        public string[] FileNames { get; set; }

        public string InitialDirectory { get; set; }

        public string Filter { get; set; }

        public string DefaultExt { get; set; }

        public bool Multiselect { get; set; }

        public bool? ShowDialog(Window owner)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = InitialDirectory,
                FileName = FileName,
                Filter = Filter,
                DefaultExt = DefaultExt,
                Multiselect = Multiselect
            };

            var result = openFileDialog.ShowDialog(owner);
            FileName = openFileDialog.FileName;
            FileNames = openFileDialog.FileNames;
            
            return result;
        }
    }
}
