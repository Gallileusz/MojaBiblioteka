using MojaBiblioteka.MVP.LoginView.View;
using System;

namespace MojaBiblioteka.MVP.LoginView.Presenter
{
    public class LoginPresenter
    {
        private readonly ILoginView _view;
        public LoginPresenter(ILoginView view)
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
            // TODO: Validate User than move to main window
            _view.CloseLoginWindow();
            _view.OpenWindow(new MainWindow());
        }

        private void RegistrationClick(object sender, EventArgs e)
        {
            _view.CloseLoginWindow();
            _view.OpenWindow(new RegistrationView.View.RegistrationView());
        }

        private void CloseClick(object sender, EventArgs e)
        {
            if (!_view.ConfirmAction("Czy na pewno chcesz wyjść z aplikacji?", "Potwierdzenie operacji"))
                return;

            _view.CloseLoginWindow();
        }
    }
}
