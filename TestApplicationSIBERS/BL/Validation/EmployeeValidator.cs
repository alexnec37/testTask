using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.Validation.EmployeeValidationRules;


namespace BL.Validation
{
    public class EmployeeValidator : ValidationBase, IValidator
    {
        private IValidationRule validationRule;
        public bool IsPropertyValid(string propertyName, object value)
        {
            RemoveErrors(propertyName);
            switch (propertyName)
            {
                case "FIO":
                    validationRule = new ValidateFIORule();
                    if (!validationRule.IsValid(value))
                    {
                        AddError(propertyName, validationRule.ErrorMessage);
                        return false;
                    }
                    break;
                case "Company":
                    validationRule = new ValidateCompanyRule();
                    if (!validationRule.IsValid(value))
                    {
                        AddError(propertyName, validationRule.ErrorMessage);
                        return false;
                    }
                    break;
            }
            return true;
        }
    }
}
