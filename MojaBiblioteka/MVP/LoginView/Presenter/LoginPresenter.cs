using MojaBiblioteka.Data.Repositories;
using MojaBiblioteka.MVP.LoginView.View;
using MojaBiblioteka.Utility;
using MojaBiblioteka.Utility.Security;
using System;

namespace MojaBiblioteka.MVP.LoginView.Presenter
{
    public class LoginPresenter
    {
        private readonly ILoginView _view;
        private readonly IUserRepository _userRepository;

        public LoginPresenter(ILoginView view, IUserRepository userRepository)
        {
            _view = view;
            _userRepository = userRepository;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _view.CloseButtonClicked += CloseClick;
            _view.RegistrationButtonClicked += RegistrationClick;
            _view.LoginButtonClicked += LoginClick;
        }

        private void LoginClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_view.Login) || string.IsNullOrWhiteSpace(_view.Password))
            {
                _view.ShowError("Login i hasło są wymagane.", "Błąd logowania");
                return;
            }

            var passwordHash = PasswordHasher.ComputeHash(_view.Password);
            var user = _userRepository.GetByCredentials(_view.Login.Trim(), passwordHash);
            if (user == null)
            {
                _view.ShowError("Niepoprawny login lub hasło.", "Błąd logowania");
                return;
            }

            UserSession.SignIn(user);
            _view.CloseWithSuccess();
            _view.CloseLoginWindow();
        }

        private void RegistrationClick(object sender, EventArgs e)
        {
            _view.OpenWindow(new RegistrationView.View.RegistrationView(_userRepository));
        }

        private void CloseClick(object sender, EventArgs e)
        {
            if (!_view.ConfirmAction("Czy na pewno chcesz wyjść z aplikacji?", "Potwierdzenie operacji"))
                return;

            _view.CloseLoginWindow();
        }
    }
}
