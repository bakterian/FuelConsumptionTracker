using System;
using System.Globalization;
using System.Windows.Controls;

namespace FCT.Control.Views.Utils
{
    public class NumericValidationRule : ValidationRule
    {
        public Type ValidationType { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);
            var validationResult = new ValidationResult(false, $"Value cannot be coverted to string.");

            if (!string.IsNullOrEmpty(strValue))
            {
                validationResult = new ValidationResult(false, $"Input should be type of {ValidationType.Name}");

                switch (ValidationType.Name)
                {
                    case "Boolean":
                        if (bool.TryParse(strValue, out var boolVal)) validationResult = new ValidationResult(true, null);
                        break;
                    case "Int32":
                        if(int.TryParse(strValue, out var intVal)) validationResult = new ValidationResult(true, null);
                        break;
                    case "UInt32":
                        if (uint.TryParse(strValue, out var uintVal)) validationResult = new ValidationResult(true, null);
                        break;
                    case "Double":
                        if(double.TryParse(strValue, out var doubleVal)) validationResult = new ValidationResult(true, null);
                        break;
                    case "Decimal":
                        if (decimal.TryParse(strValue, out var decimalVal)) validationResult = new ValidationResult(true, null);
                        break;
                    case "Int64":
                        if (long.TryParse(strValue, out var longVal)) validationResult = new ValidationResult(true, null);
                        break;
                    default:
                        throw new InvalidCastException($"{ValidationType.Name} is not supported");
                }
            }

            return validationResult;
        }
    }
}
