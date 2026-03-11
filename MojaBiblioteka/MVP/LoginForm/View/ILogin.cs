using System;
using System.Windows;

namespace MojaBiblioteka.MVP.LoginForm.View
{
    public interface ILogin
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
