using System;

namespace ValidationLibrary
{
    /// <summary>
    /// Interface for all validators in the library
    /// </summary>
    /// <typeparam name="T">The type of value to validate</typeparam>
    public interface IValidator<T>
    {
        /// <summary>
        /// Validates the given value
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>Validation result containing success status and any error messages</returns>
        ValidationResult Validate(T value);
    }

    /// <summary>
    /// Represents the result of a validation operation
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets whether the validation was successful
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the error message if validation failed, null otherwise
        /// </summary>
        public string? ErrorMessage { get; }

        private ValidationResult(bool isValid, string? errorMessage = null)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Creates a successful validation result
        /// </summary>
        public static ValidationResult Success() => new(true);

        /// <summary>
        /// Creates a failed validation result with the specified error message
        /// </summary>
        /// <param name="errorMessage">The error message describing why validation failed</param>
        public static ValidationResult Failure(string errorMessage) => new(false, errorMessage);
    }
} 