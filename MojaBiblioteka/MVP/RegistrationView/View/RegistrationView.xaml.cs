using MojaBiblioteka.MVP.RegistrationView.Presenter;
using System;
using System.Windows;
using MojaBiblioteka.Data.Repositories;

namespace MojaBiblioteka.MVP.RegistrationView.View
{
    public partial class RegistrationView : Window, IRegistrationView
    {
        public event EventHandler ReturnToLoginLabelCicked;
        public event EventHandler CloseButtonClicked;
        public event EventHandler RegisterButtonClicked;

        public string Login => txtLogin.Text;

        public string Password => txtPassword.Password;

        public string ConfirmationPassword => txtConfirmationPassword.Password;

        private readonly RegistrationPresenter _presenter;

        public RegistrationView(IUserRepository userRepository)
        {
            InitializeComponent();
            _presenter = new RegistrationPresenter(this, userRepository);
        }

        public void lblClose_Click(object sender, RoutedEventArgs e) => CloseButtonClicked?.Invoke(sender, e);

        public void btnRegister_Click(object sender, RoutedEventArgs e) => RegisterButtonClicked?.Invoke(sender, e);

        public void lblReturnToLoginForm_Click(object sender, RoutedEventArgs e) => ReturnToLoginLabelCicked?.Invoke(sender, e);

        public void CloseThisView() => this.Close();

        public void ShowMessage(string message, string title) => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

        public void ShowError(string message, string title) => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
