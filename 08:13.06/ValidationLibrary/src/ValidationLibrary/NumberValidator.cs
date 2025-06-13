using System;

namespace ValidationLibrary
{
    /// <summary>
    /// Validator for numeric values
    /// </summary>
    public class NumberValidator : IValidator<double>
    {
        private double? _minValue;
        private double? _maxValue;
        private bool _isInteger;
        private string? _customErrorMessage;

        /// <summary>
        /// Sets the minimum value requirement
        /// </summary>
        /// <param name="value">Minimum allowed value</param>
        public NumberValidator Min(double value)
        {
            _minValue = value;
            return this;
        }

        /// <summary>
        /// Sets the maximum value requirement
        /// </summary>
        /// <param name="value">Maximum allowed value</param>
        public NumberValidator Max(double value)
        {
            _maxValue = value;
            return this;
        }

        /// <summary>
        /// Requires the number to be an integer
        /// </summary>
        public NumberValidator Integer()
        {
            _isInteger = true;
            return this;
        }

        /// <summary>
        /// Sets a custom error message to use when validation fails
        /// </summary>
        /// <param name="message">Custom error message</param>
        public NumberValidator WithMessage(string message)
        {
            _customErrorMessage = message;
            return this;
        }

        /// <summary>
        /// Validates the numeric value against all configured rules
        /// </summary>
        /// <param name="value">The number to validate</param>
        public ValidationResult Validate(double value)
        {
            if (_minValue.HasValue && value < _minValue.Value)
            {
                return ValidationResult.Failure(_customErrorMessage ?? 
                    $"Value must be at least {_minValue.Value}");
            }

            if (_maxValue.HasValue && value > _maxValue.Value)
            {
                return ValidationResult.Failure(_customErrorMessage ?? 
                    $"Value must not exceed {_maxValue.Value}");
            }

            if (_isInteger && Math.Floor(value) != value)
            {
                return ValidationResult.Failure(_customErrorMessage ?? 
                    "Value must be an integer");
            }

            return ValidationResult.Success();
        }
    }
} 