using System.Windows;

namespace GpsIntersect.Dialogs
{
    public class MessageBoxDialog : IIMessageBoxDialog
    {
        public void ShowDialog(string text, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            MessageBox.Show(text, caption, button, icon);
        }
    }
}
