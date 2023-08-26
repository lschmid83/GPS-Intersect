using System.Windows;

namespace GpsIntersect.Dialogs
{
    public interface IIMessageBoxDialog
    {
        void ShowDialog(string text, string caption, MessageBoxButton button, MessageBoxImage icon);
    }
}
