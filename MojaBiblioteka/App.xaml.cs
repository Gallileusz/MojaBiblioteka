using MojaBiblioteka.MVP.LoginForm.View;
using System.Windows;

namespace MojaBiblioteka
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var login = new Login();
            login.ShowDialog();

            if (login.DialogResult == true)
            {
                var mainWindow = new MainWindow();
                MainWindow = mainWindow;
                ShutdownMode = ShutdownMode.OnMainWindowClose;
                mainWindow.Show();
            }
            else
                Shutdown();
        }
    }
}
