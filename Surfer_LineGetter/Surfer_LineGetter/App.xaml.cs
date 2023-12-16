using LineGetterCore.Configuration;
using LineGetterModel.ValidateService;
using Surfer_LineGetter.AppService;
using System.Linq;
using System.Windows;

namespace Surfer_LineGetter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppData.InputFileInformation = InputFileChecker.CheckAndGet(true);

            if (AppData.InputFileInformation == null)
            {
                Shutdown();
                return;
            }

            if (AppData.InputFileInformation.Where(x => x.IsValide == true).Count() == 0)
            {
                MessageBox.Show("Пригодных для открытия файлов в папке " + DirectoryConfiguration.InputDir + " не обнаружено.\n" +
                    "Программа будет остановлена.");
                Shutdown();
                return;
            }

            WindowsObjects.AppWindow = new();
            if (WindowsObjects.AppWindow.ShowDialog() == true)
            {
                WindowsObjects.AppWindow.Show();
            }
            else
            {
                WindowsObjects.AppWindow = null;
                Shutdown();
                return;
            }
        }
    }
}