using System;
using System.Collections.Generic;
using System.Linq;

namespace ValidationLibrary
{
    /// <summary>
    /// Validator for arrays and collections
    /// </summary>
    /// <typeparam name="T">The type of items in the array</typeparam>
    public class ArrayValidator<T> : IValidator<IEnumerable<T>>
    {
        private readonly IValidator<T> _itemValidator;
        private int? _minLength;
        private int? _maxLength;
        private string? _customErrorMessage;

        /// <summary>
        /// Creates a new array validator that validates each item using the specified validator
        /// </summary>
        /// <param name="itemValidator">Validator to use for each item</param>
        public ArrayValidator(IValidator<T> itemValidator)
        {
            _itemValidator = itemValidator ?? throw new ArgumentNullException(nameof(itemValidator));
        }

        /// <summary>
        /// Sets the minimum length requirement
        /// </summary>
        /// <param name="length">Minimum allowed length</param>
        public ArrayValidator<T> MinLength(int length)
        {
            _minLength = length;
            return this;
        }

        /// <summary>
        /// Sets the maximum length requirement
        /// </summary>
        /// <param name="length">Maximum allowed length</param>
        public ArrayValidator<T> MaxLength(int length)
        {
            _maxLength = length;
            return this;
        }

        /// <summary>
        /// Sets a custom error message to use when validation fails
        /// </summary>
        /// <param name="message">Custom error message</param>
        public ArrayValidator<T> WithMessage(string message)
        {
            _customErrorMessage = message;
            return this;
        }

        /// <summary>
        /// Validates the array against all configured rules
        /// </summary>
        /// <param name="value">The array to validate</param>
        public ValidationResult Validate(IEnumerable<T> value)
        {
            if (value == null)
            {
                return ValidationResult.Failure(_customErrorMessage ?? "Array cannot be null");
            }

            var items = value.ToList();

            if (_minLength.HasValue && items.Count < _minLength.Value)
            {
                return ValidationResult.Failure(_customErrorMessage ?? 
                    $"Array must contain at least {_minLength.Value} items");
            }

            if (_maxLength.HasValue && items.Count > _maxLength.Value)
            {
                return ValidationResult.Failure(_customErrorMessage ?? 
                    $"Array must not contain more than {_maxLength.Value} items");
            }

            var errors = new List<string>();
            for (int i = 0; i < items.Count; i++)
            {
                var result = _itemValidator.Validate(items[i]);
                if (!result.IsValid)
                {
                    errors.Add($"Item at index {i}: {result.ErrorMessage}");
                }
            }

            if (errors.Any())
            {
                return ValidationResult.Failure(_customErrorMessage ?? 
                    string.Join(Environment.NewLine, errors));
            }

            return ValidationResult.Success();
        }
    }
} 