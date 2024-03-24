using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DoAn1.UI.ValidationRules
{
    internal class LengthValidationRule : ValidationRule
    {
        public int? MinLength { get; set; } = 0;
        public int? MaxLength { get; set; } = null;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (string)value;

            if (MinLength != null && MaxLength != null)
            {
                if (text.Length < MinLength || text.Length > MaxLength) return new ValidationResult(false, $"Số ký tự phải trong khoảng [{MinLength}-{MaxLength}].");
            }

            else if (MinLength != null)
            {
                if (text.Length < MinLength) return new ValidationResult(false, $"Phải nhập tối thiểu {MinLength} ký tự.");
            }
            else if (MaxLength != null)
            {
                if (text.Length > MaxLength) return new ValidationResult(false, $"Chỉ phép nhập tối đa {MaxLength} ký tự.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
