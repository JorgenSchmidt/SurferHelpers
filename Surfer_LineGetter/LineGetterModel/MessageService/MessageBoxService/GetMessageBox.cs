using System.Windows;

namespace LineGetterModel.MessageService.MessageBoxService
{
    public static class GetMessageBox
    {
        public static void Show(string Message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(Message);
            });
        }
    }
}