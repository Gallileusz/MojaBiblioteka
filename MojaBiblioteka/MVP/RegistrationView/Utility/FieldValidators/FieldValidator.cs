using System.Collections.Generic;

namespace MojaBiblioteka.MVP.RegistrationView.Utility.FieldValidators
{
    public class FieldValidator
    {
        private IEnumerable<IFieldValidator> _fields;

        public FieldValidator(IEnumerable<IFieldValidator> fields)
        {
            _fields = fields;
        }

        public string Validate()
        {
            foreach (var field in _fields)
            {
                var error = field.IsInvalid();

                if (!string.IsNullOrEmpty(error))
                    return error;
            }

            return string.Empty;
        }
    }
}
