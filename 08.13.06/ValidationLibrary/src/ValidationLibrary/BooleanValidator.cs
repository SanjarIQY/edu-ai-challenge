using System;

namespace ValidationLibrary
{
    /// <summary>
    /// Validator for boolean values
    /// </summary>
    public class BooleanValidator : IValidator<bool>
    {
        private bool? _requiredValue;
        private string? _customErrorMessage;

        /// <summary>
        /// Requires the boolean to have a specific value
        /// </summary>
        /// <param name="value">The required boolean value</param>
        public BooleanValidator Required(bool value)
        {
            _requiredValue = value;
            return this;
        }

        /// <summary>
        /// Sets a custom error message to use when validation fails
        /// </summary>
        /// <param name="message">Custom error message</param>
        public BooleanValidator WithMessage(string message)
        {
            _customErrorMessage = message;
            return this;
        }

        /// <summary>
        /// Validates the boolean value against all configured rules
        /// </summary>
        /// <param name="value">The boolean to validate</param>
        public ValidationResult Validate(bool value)
        {
            if (_requiredValue.HasValue && value != _requiredValue.Value)
            {
                return ValidationResult.Failure(_customErrorMessage ?? 
                    $"Value must be {_requiredValue.Value}");
            }

            return ValidationResult.Success();
        }
    }
} 