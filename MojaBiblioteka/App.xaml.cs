using MojaBiblioteka.Data.Repositories;
using MojaBiblioteka.MVP.LoginView.View;
using MojaBiblioteka.MVP.MainView.View;
using MojaBiblioteka.Utility;
using System;
using System.IO;
using System.Windows;

namespace MojaBiblioteka
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            UserSession.SignOut();

            var dbDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MojaBiblioteka");
            var databasePath = Path.Combine(dbDirectory, "moja-biblioteka.sqlite");

            var userRepository = new UserRepository(databasePath);
            var login = new LoginView(userRepository);
            login.ShowDialog();

            if (login.DialogResult == true && UserSession.IsAuthenticated)
            {
                var mainWindow = new MainView(UserSession.UserId, UserSession.Login);
                MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
                Shutdown();
        }
    }
}
