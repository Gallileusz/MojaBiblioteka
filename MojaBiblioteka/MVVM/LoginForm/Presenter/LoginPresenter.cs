using MojaBiblioteka.MVVM.LoginForm.View;
using System;

namespace MojaBiblioteka.MVVM.LoginForm.Presenter
{
    public class LoginPresenter
    {
        private readonly ILogin _view;
        public LoginPresenter(ILogin view)
        {
            _view = view;

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
            throw new NotImplementedException();
        }

        private void RegistrationClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CloseClick(object sender, EventArgs e)
        {
            if (!_view.ConfirmAction("Czy na pewno chcesz wyjść z aplikacji?", "Potwierdzenie operacji"))
                return;

            _view.CloseThisWindow();
        }
    }
}
