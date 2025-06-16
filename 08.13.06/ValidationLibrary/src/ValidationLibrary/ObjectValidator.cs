using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ValidationLibrary
{
    /// <summary>
    /// Validator for complex objects
    /// </summary>
    /// <typeparam name="T">The type of object to validate</typeparam>
    public class ObjectValidator<T> : IValidator<T> where T : class
    {
        private readonly Dictionary<string, IValidator<object>> _schema;
        private string? _customErrorMessage;

        /// <summary>
        /// Creates a new object validator with the specified property validators
        /// </summary>
        /// <param name="schema">Dictionary of property names to their validators</param>
        public ObjectValidator(Dictionary<string, IValidator<object>> schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        /// <summary>
        /// Sets a custom error message to use when validation fails
        /// </summary>
        /// <param name="message">Custom error message</param>
        public ObjectValidator<T> WithMessage(string message)
        {
            _customErrorMessage = message;
            return this;
        }

        /// <summary>
        /// Validates the object against all configured property validators
        /// </summary>
        /// <param name="value">The object to validate</param>
        public ValidationResult Validate(T value)
        {
            if (value == null)
            {
                return ValidationResult.Failure(_customErrorMessage ?? "Object cannot be null");
            }

            var errors = new List<string>();
            var type = typeof(T);

            foreach (var (propertyName, validator) in _schema)
            {
                var property = type.GetProperty(propertyName);
                if (property == null)
                {
                    throw new ArgumentException($"Property '{propertyName}' not found on type {type.Name}");
                }

                var propertyValue = property.GetValue(value);
                var result = validator.Validate(propertyValue);

                if (!result.IsValid)
                {
                    errors.Add($"{propertyName}: {result.ErrorMessage}");
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