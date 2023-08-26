using System.Windows;

namespace GpsIntersect.Dialogs
{
    public class SaveFileDialog : ISaveFileDialog
    {
        public string FileName { get; set; }

        public string[] FileNames { get; set; }

        public string InitialDirectory { get; set; }

        public string Filter { get; set; }

        public string DefaultExt { get; set; }

        public bool? ShowDialog(Window owner)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                InitialDirectory = InitialDirectory,
                FileName = FileName,
                Filter = Filter,
                DefaultExt = DefaultExt       
            };

            var result = saveFileDialog.ShowDialog(owner);
            FileName = saveFileDialog.FileName;
            return result;
        }
    }
}
