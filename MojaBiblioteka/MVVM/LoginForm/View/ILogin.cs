using System;
using System.Windows;

namespace MojaBiblioteka.MVVM.LoginForm.View
{
    public interface ILogin
    {
        event EventHandler LoginButtonClicked;
        event EventHandler RegistrationButtonClicked;
        event EventHandler CloseButtonClicked;

        bool ConfirmAction(string message, string title);
        void ShowMessage(string message, string title);
        void CloseThisWindow();
        void OpenWindow(Window window);
    }
}
