using MojaBiblioteka.MVP.RegistrationView.View;
using System;

namespace MojaBiblioteka.MVP.RegistrationView.Presenter
{
    public class RegistrationPresenter
    {
        private readonly IRegistrationView _view;
        public RegistrationPresenter(IRegistrationView view)
        {
            _view = view;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _view.ReturnToLoginLabelCicked += ReturnToLogin;
            _view.CloseButtonClicked += CloseApp;
            _view.RegisterButtonClicked += RegisterNewUser;
        }

        private void RegisterNewUser(object sender, EventArgs e)
        {
            // jeszcze dodaj walidacje czy nie istnieje encja z tym samym loginem (musi być unikatowe)

            var error = ValidateNewUser();
            if (!string.IsNullOrEmpty(error))
            {
                _view.ShowError(error, "Błąd!");
                return;
            }

            // insert

            _view.ShowMessage("Pomyślnie dodano nowego użytkownika", "Sukces");
        }

        private void CloseApp(object sender, EventArgs e)
        {
            _view.CloseThisView();
        }

        private void ReturnToLogin(object sender, EventArgs e)
        {
            _view.OpenLoginView();
            _view.CloseThisView();
        }

        private string ValidateNewUser()
        {
            if (string.IsNullOrWhiteSpace(_view.Login))
                return "Pole login nie może być puste!";

            if (_view.Login.Length < 5)
                return "Login musi się składać z przynajmniej 5 znaków!";

            if (string.IsNullOrEmpty(_view.Password) || string.IsNullOrEmpty(_view.ConfirmationPassword))
                return "Pola haseł nie mogą być puste!";

            if (_view.Password != _view.ConfirmationPassword)
                return "Hasła różnią się od siebie!";

            if (_view.Password.Length < 5)
                return "Hasło musi się składać z przynajmniej 5 znaków!";

            return string.Empty; // Sukces
        }
    }
}
