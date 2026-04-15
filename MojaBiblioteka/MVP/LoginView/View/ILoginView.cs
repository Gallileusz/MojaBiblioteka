using System;
using System.Windows;

namespace MojaBiblioteka.MVP.LoginView.View
{
    public interface ILoginView
    {
        string Login { get; }
        string Password { get; }

        event EventHandler LoginButtonClicked;
        event EventHandler RegistrationButtonClicked;
        event EventHandler CloseButtonClicked;

        bool ConfirmAction(string message, string title);
        void ShowMessage(string message, string title);
        void ShowError(string message, string title);
        void CloseLoginWindow();
        void CloseWithSuccess();
        void OpenWindow(Window window);
    }
}
