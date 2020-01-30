using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Validation
{
    public interface IValidationRule
    {
        bool IsValid(object value);
        string ErrorMessage { get; }
    }
}
