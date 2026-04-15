using MojaBiblioteka.Data.Repositories;
using MojaBiblioteka.MVP.RegistrationView.Utility.FieldValidators;
using MojaBiblioteka.MVP.RegistrationView.Utility.FieldValidators.Fields;
using MojaBiblioteka.MVP.RegistrationView.View;
using MojaBiblioteka.Utility.Security;
using System;
using System.Collections.Generic;

namespace MojaBiblioteka.MVP.RegistrationView.Presenter
{
    public class RegistrationPresenter
    {
        private readonly IRegistrationView _view;
        private readonly IUserRepository _userRepository;

        public RegistrationPresenter(IRegistrationView view, IUserRepository userRepository)
        {
            _view = view;
            _userRepository = userRepository;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _view.ReturnToLoginLabelCicked += Return;
            _view.CloseButtonClicked += Return;
            _view.RegisterButtonClicked += RegisterNewUser;
        }

        private void RegisterNewUser(object sender, EventArgs e)
        {
            GetFieldValidator(out var fieldValidator);
            var error = fieldValidator.Validate();
            if (!string.IsNullOrEmpty(error))
            {
                _view.ShowError(error, "Błąd!"); return;
            }

            var passwordHash = PasswordHasher.ComputeHash(_view.Password);
            _userRepository.Add(_view.Login.Trim(), passwordHash);

            _view.ShowMessage("Pomyślnie dodano nowego użytkownika", "Sukces");
            _view.CloseThisView();
        }

        private void Return(object sender, EventArgs e)
        {
            _view.CloseThisView();
        }

        private void GetFieldValidator(out FieldValidator fieldValidator)
        {
            var validatores = new List<IFieldValidator>()
            {
                new LoginValidator(_view, _userRepository),
                new PasswordValidator(_view)
            };

            fieldValidator = new FieldValidator(validatores);
        }
    }
}