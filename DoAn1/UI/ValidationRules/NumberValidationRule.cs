using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace DoAn1.UI.ValidationRules
{
    internal class NumberValidationRule : ValidationRule
    {
        public float? Min { get; set; } = null;
        public float? Max { get; set; } = null;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = (string)value;
            string numberPattern = @"^[-+]?[0-9]*\.?[0-9]+$";

            if (Regex.IsMatch(input, numberPattern))
            {
                float number = (float)value;

                if (Min != null && Max != null)
                {
                    if (number < Min || number > Max) return new ValidationResult(false, $"Giá trị phải trong khoảng [{Min}-{Max}].");
                }

                else if (Min != null)
                {
                    if (number < Min) return new ValidationResult(false, $"Giá trị phải lớn hơn hoặc bằng {Min}");
                }
                else if (Max != null)
                {
                    if (number > Max) return new ValidationResult(false, $"Giá trị phải bé hơn hoặc bằng {Max}");
                }

                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Chỉ được nhập số");
        }
    }
}
