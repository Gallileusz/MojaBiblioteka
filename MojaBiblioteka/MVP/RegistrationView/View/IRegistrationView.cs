using System;

namespace MojaBiblioteka.MVP.RegistrationView.View
{
    public interface IRegistrationView
    {
        string Login { get; }
        string Password { get; }
        string ConfirmationPassword { get; }

        event EventHandler ReturnToLoginLabelCicked;
        event EventHandler CloseButtonClicked;
        event EventHandler RegisterButtonClicked;

        void CloseThisView();
        void OpenLoginView();
        void ShowMessage(string message, string title);
        void ShowError(string message, string title);
    }
}
