using MojaBiblioteka.Data.Repositories;
using MojaBiblioteka.MVP.RegistrationView.View;

namespace MojaBiblioteka.MVP.RegistrationView.Utility.FieldValidators.Fields
{
    public class LoginValidator : IFieldValidator
    {
        private readonly IRegistrationView _view;
        private readonly IUserRepository _userRepository;

        public LoginValidator(IRegistrationView view, IUserRepository userRepository)
        {
            _view = view;
            _userRepository = userRepository;
        }

        public string IsInvalid()
        {
            var isLoginEmptyError = IsLoginEmpty();
            if (!string.IsNullOrEmpty(isLoginEmptyError))
                return isLoginEmptyError;

            var isLoginTooShortError = IsLoginTooShort();
            if (!string.IsNullOrEmpty(isLoginTooShortError))
                return isLoginTooShortError;

            var isLoginAlreadyInUseError = IsLoginAlreadyInUse();
            if (!string.IsNullOrEmpty(isLoginAlreadyInUseError))
                return isLoginAlreadyInUseError;

            return string.Empty;
        }

        private string IsLoginEmpty() => string.IsNullOrWhiteSpace(_view.Login)
            ? "Pole login nie może być puste!"
            : string.Empty;


        private string IsLoginTooShort() => _view.Login.Length < 5
            ? "Login musi się składać z przynajmniej 5 znaków!"
            : string.Empty;

        private string IsLoginAlreadyInUse()
        {
            return _userRepository.Exists(_view.Login)
                ? "Ten login jest już zajęty."
                : string.Empty;
        }
    }
}
