using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Validation.EmployeeValidationRules
{
    public class ValidateCompanyRule : IValidationRule
    {
        public bool IsValid(object value)
        {
            _errName = null;
            if (value == null)
                _errName = "Не указана компания!";
            return String.IsNullOrEmpty(_errName);
        }

        private string _errName;
        public string ErrorMessage
        { get { return _errName; } }
    }
}
