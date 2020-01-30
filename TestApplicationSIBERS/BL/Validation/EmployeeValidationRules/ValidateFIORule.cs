using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BL.Validation.EmployeeValidationRules
{
    public class ValidateFIORule : IValidationRule
    {
        public bool IsValid(object value)
        {
            _errName = null;
            if (string.IsNullOrWhiteSpace((string)value))
                _errName = "ФИО не введено";
            else
            {
                if (((string)value).Length < 2)
                    _errName = "ФИО меньше 2х символов";
                if (((string)value).Length > 70)
                    _errName = "ФИО больше 50х символов";
                if (!Regex.IsMatch((string)value, @"[^\d\s]"))
                {
                    string err = "ФИО не может состоять из цифр и пробелов";
                    if (!String.IsNullOrEmpty(_errName))
                        _errName = _errName + "\n";
                    _errName += err;
                }
            }
            return String.IsNullOrEmpty(_errName);
        }

        private string _errName;
        public string ErrorMessage
        { get { return _errName; } }
    }
}
