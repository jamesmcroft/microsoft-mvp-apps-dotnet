namespace MVP.App.ValidationRules
{
    using WinUX.Data.Validation;

    /// <summary>
    /// Defines a validation rule for validating a positive integer value.
    /// </summary>
    public class PositiveIntValidationRule : ValidationRule
    {
        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            var s = value.ToString();
            if (string.IsNullOrWhiteSpace(s))
            {
                return true;
            }

            int result;
            var parsed = int.TryParse(s, out result);
            return parsed && result >= 0;
        }
    }
}