using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DoAn1.UI.ValidationRules
{
    internal class NotEmptyOrWhiteSpacesOnlyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (string)value;

            if (text.Trim().Length == 0)
            {
                return new ValidationResult(false, "Không được để trống.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
