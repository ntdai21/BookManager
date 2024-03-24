using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DoAn1.UI.ValidationRules
{
    internal class AlphabetsOnlyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (string)value;

            if (text.All(Char.IsLetter)) return ValidationResult.ValidResult;

            else return new ValidationResult(false, "Chỉ được nhập chữ.");
        }
    }
}
