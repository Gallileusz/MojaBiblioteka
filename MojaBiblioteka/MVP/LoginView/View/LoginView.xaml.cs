using MojaBiblioteka.MVP.LoginView.Presenter;
using System;
using System.Windows;

namespace MojaBiblioteka.MVP.LoginView.View
{
    public partial class LoginView : Window, ILoginView
    {
        public event EventHandler LoginButtonClicked;
        public event EventHandler RegistrationButtonClicked;
        public event EventHandler CloseButtonClicked;

        private readonly LoginPresenter _presenter;
        public LoginView()
        {
            InitializeComponent();
            _presenter = new LoginPresenter(this);
        }

        #region Events

        private void Button_Login_Click(object sender, RoutedEventArgs e) => LoginButtonClicked?.Invoke(sender, e);

        private void lblRegister_Click(object sender, RoutedEventArgs e) => RegistrationButtonClicked?.Invoke(sender, e);

        private void lblClose_Click(object sender, RoutedEventArgs e) => CloseButtonClicked?.Invoke(sender, e);

        #endregion

        public bool ConfirmAction(string message, string title) => MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes;

        public void ShowMessage(string message, string title) => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

        public void CloseLoginWindow() => this.Close();

        public void OpenWindow(Window window) => window.ShowDialog();
    }
}
