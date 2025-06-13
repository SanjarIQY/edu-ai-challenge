using System;
using System.Text.RegularExpressions;

namespace ValidationLibrary
{
    /// <summary>
    /// Validator for string values
    /// </summary>
    public class StringValidator : IValidator<string>
    {
        private int? _minLength;
        private int? _maxLength;
        private string? _pattern;
        private string? _customErrorMessage;

        /// <summary>
        /// Sets the minimum length requirement
        /// </summary>
        /// <param name="length">Minimum allowed length</param>
        public StringValidator MinLength(int length)
        {
            _minLength = length;
            return this;
        }

        /// <summary>
        /// Sets the maximum length requirement
        /// </summary>
        /// <param name="length">Maximum allowed length</param>
        public StringValidator MaxLength(int length)
        {
            _maxLength = length;
            return this;
        }

        /// <summary>
        /// Sets a regex pattern that the string must match
        /// </summary>
        /// <param name="pattern">Regex pattern to match</param>
        public StringValidator Pattern(string pattern)
        {
            _pattern = pattern;
            return this;
        }

        /// <summary>
        /// Sets a custom error message to use when validation fails
        /// </summary>
        /// <param name="message">Custom error message</param>
        public StringValidator WithMessage(string message)
        {
            _customErrorMessage = message;
            return this;
        }

        /// <summary>
        /// Validates the string value against all configured rules
        /// </summary>
        /// <param name="value">The string to validate</param>
        public ValidationResult Validate(string value)
        {
            if (value == null)
            {
                return ValidationResult.Failure(_customErrorMessage ?? "Value cannot be null");
            }

            if (_minLength.HasValue && value.Length < _minLength.Value)
            {
                return ValidationResult.Failure(_customErrorMessage ?? 
                    $"String length must be at least {_minLength.Value} characters");
            }

            if (_maxLength.HasValue && value.Length > _maxLength.Value)
            {
                return ValidationResult.Failure(_customErrorMessage ?? 
                    $"String length must not exceed {_maxLength.Value} characters");
            }

            if (_pattern != null && !Regex.IsMatch(value, _pattern))
            {
                return ValidationResult.Failure(_customErrorMessage ?? 
                    "String does not match required pattern");
            }

            return ValidationResult.Success();
        }
    }
} 