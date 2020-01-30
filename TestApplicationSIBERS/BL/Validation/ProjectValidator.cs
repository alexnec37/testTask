using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainEntity;
using System.Text.RegularExpressions;
using BL.Validation.ProjectValidationRules;

namespace BL.Validation
{
    public class ProjectValidator : ValidationBase, IValidator
    {
        private IValidationRule validationRule;
        public bool IsPropertyValid(string propertyName, object value)
        {
            RemoveErrors(propertyName);
            switch (propertyName)
            {
                case "NameProject":
                    validationRule = new ValidateNameProjectRule();
                    if (!validationRule.IsValid(value))
                    {
                        AddError(propertyName, validationRule.ErrorMessage);
                        return false;
                    }
                    break;
                case "Customer":
                    validationRule = new ValidateCustomerRule();
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
