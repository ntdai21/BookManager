using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DoAn1.UI.ValidationRules
{
    internal class DigitsOnlyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (string)value;

            if (text.All(Char.IsDigit)) return ValidationResult.ValidResult;

            return new ValidationResult(false, "Digits only allowed.");
        }
    }
}
