namespace MVP.App.ValidationRules
{
    using WinUX.Data.Validation;

    /// <summary>
    /// Defines a validation rule for validating a positive integer value.
    /// </summary>
    public class PositiveIntValidationRule : ValidationRule
    {
        /// <summary>
        /// Checks whether the given value is valid according to the rule.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>Returns true if the value is valid.</returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            string valueString = value.ToString();
            if (string.IsNullOrWhiteSpace(valueString))
            {
                return true;
            }

            bool parsed = int.TryParse(valueString, out int result);
            return parsed && result >= 0;
        }
    }
}