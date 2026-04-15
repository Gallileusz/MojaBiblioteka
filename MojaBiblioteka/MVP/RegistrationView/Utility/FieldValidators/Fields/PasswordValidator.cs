using MojaBiblioteka.MVP.RegistrationView.View;

namespace MojaBiblioteka.MVP.RegistrationView.Utility.FieldValidators.Fields
{
    public class PasswordValidator : IFieldValidator
    {
        private readonly IRegistrationView _view;

        public PasswordValidator(IRegistrationView view)
        {
            _view = view;
        }

        public string IsInvalid()
        {
            var isPasswordEmpty = IsPasswordEmpty();
            if (!string.IsNullOrEmpty(isPasswordEmpty))
                return isPasswordEmpty;

            var isPasswordTooShort = IsPasswordTooShort();
            if (!string.IsNullOrEmpty(isPasswordTooShort))
                return isPasswordTooShort;

            var arePasswordsDifferent = ArePasswordsDifferent();
            if (!string.IsNullOrEmpty(arePasswordsDifferent))
                return arePasswordsDifferent;

            return string.Empty;
        }

        private string IsPasswordEmpty() => string.IsNullOrEmpty(_view.Password) || string.IsNullOrEmpty(_view.ConfirmationPassword)
            ? "Pola haseł nie mogą być puste!"
            : string.Empty;

        private string IsPasswordTooShort() => _view.Password.Length < 5
            ? "Hasło musi się składać z przynajmniej 5 znaków!"
            : string.Empty;

        private string ArePasswordsDifferent() => _view.Password != _view.ConfirmationPassword
            ? "Hasła różnią się od siebie!"
            : string.Empty;
    }
}
