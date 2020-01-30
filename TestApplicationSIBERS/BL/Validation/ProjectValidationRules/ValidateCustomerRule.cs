using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Validation.ProjectValidationRules
{
    public class ValidateCustomerRule : IValidationRule
    {
        public bool IsValid(object value)
        {
            _errName = null;
            if (value == null)
                _errName = "Не указан заказчик!";
            return String.IsNullOrEmpty(_errName);
        }

        private string _errName;
        public string ErrorMessage
        { get { return _errName; } }
    }
}
