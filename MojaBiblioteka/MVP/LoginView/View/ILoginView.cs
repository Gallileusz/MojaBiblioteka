using System;
using System.Windows;

namespace MojaBiblioteka.MVP.LoginView.View
{
    public interface ILoginView
    {
        event EventHandler LoginButtonClicked;
        event EventHandler RegistrationButtonClicked;
        event EventHandler CloseButtonClicked;

        bool ConfirmAction(string message, string title);
        void ShowMessage(string message, string title);
        void CloseLoginWindow();
        void OpenWindow(Window window);
    }
}
