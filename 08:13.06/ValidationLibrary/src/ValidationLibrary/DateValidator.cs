using System;

namespace ValidationLibrary
{
    /// <summary>
    /// Validator for date values
    /// </summary>
    public class DateValidator : IValidator<DateTime>
    {
        private DateTime? _minDate;
        private DateTime? _maxDate;
        private string? _customErrorMessage;

        /// <summary>
        /// Sets the minimum date requirement
        /// </summary>
        /// <param name="date">Minimum allowed date</param>
        public DateValidator Min(DateTime date)
        {
            _minDate = date;
            return this;
        }

        /// <summary>
        /// Sets the maximum date requirement
        /// </summary>
        /// <param name="date">Maximum allowed date</param>
        public DateValidator Max(DateTime date)
        {
            _maxDate = date;
            return this;
        }

        /// <summary>
        /// Sets a custom error message to use when validation fails
        /// </summary>
        /// <param name="message">Custom error message</param>
        public DateValidator WithMessage(string message)
        {
            _customErrorMessage = message;
            return this;
        }

        /// <summary>
        /// Validates the date value against all configured rules
        /// </summary>
        /// <param name="value">The date to validate</param>
        public ValidationResult Validate(DateTime value)
        {
            if (_minDate.HasValue && value < _minDate.Value)
            {
                return ValidationResult.Failure(_customErrorMessage ?? 
                    $"Date must be on or after {_minDate.Value:d}");
            }

            if (_maxDate.HasValue && value > _maxDate.Value)
            {
                return ValidationResult.Failure(_customErrorMessage ?? 
                    $"Date must be on or before {_maxDate.Value:d}");
            }

            return ValidationResult.Success();
        }
    }
} 