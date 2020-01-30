using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BL.Validation.ProjectValidationRules
{
    public class ValidateNameProjectRule : IValidationRule
    {
        public bool IsValid(object value)
        {
            _errName = null;
            if (string.IsNullOrWhiteSpace((string)value))
                _errName = "Наименование не введено";
            else
            {
                if (((string)value).Length < 2)
                    _errName = "Наименование проекта меньше 2х символов";
                if (((string)value).Length > 50)
                    _errName = "Наименование проекта больше 50х символов";
                if (!Regex.IsMatch((string)value, @"[^\d\s]"))
                {
                    string err = "Название проекта не может состоять только из цифр и пробелов";
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
