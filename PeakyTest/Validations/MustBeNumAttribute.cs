using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PeakyTestUI.Validations
{
    public class MustBeNumAttribute : ValidationAttribute
    {
        public MustBeNumAttribute()
        {
            ErrorMessage = "Введите целое число";
        }

        public override bool IsValid(object? value)
        {
            if (value is not string str || string.IsNullOrWhiteSpace(str))
                return true;

            return int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out _);
        }
    }
}
