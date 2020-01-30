using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Validation
{
    public interface IValidator
    {
        bool IsPropertyValid(string propertyName, object value);
        string GetErrorMessageForProperty(string propertyName);
        bool HasErrors();
    }
}
