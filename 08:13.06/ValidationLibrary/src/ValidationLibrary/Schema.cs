using System;
using System.Collections.Generic;

namespace ValidationLibrary
{
    /// <summary>
    /// Factory class for creating validators
    /// </summary>
    public static class Schema
    {
        /// <summary>
        /// Creates a string validator
        /// </summary>
        public static StringValidator String() => new();

        /// <summary>
        /// Creates a number validator
        /// </summary>
        public static NumberValidator Number() => new();

        /// <summary>
        /// Creates a boolean validator
        /// </summary>
        public static BooleanValidator Boolean() => new();

        /// <summary>
        /// Creates a date validator
        /// </summary>
        public static DateValidator Date() => new();

        /// <summary>
        /// Creates an object validator with the specified property validators
        /// </summary>
        /// <typeparam name="T">The type of object to validate</typeparam>
        /// <param name="schema">Dictionary of property names to their validators</param>
        public static ObjectValidator<T> Object<T>(Dictionary<string, IValidator<object>> schema) where T : class
            => new(schema);

        /// <summary>
        /// Creates an array validator that validates each item using the specified validator
        /// </summary>
        /// <typeparam name="T">The type of items in the array</typeparam>
        /// <param name="itemValidator">Validator to use for each item</param>
        public static ArrayValidator<T> Array<T>(IValidator<T> itemValidator)
            => new(itemValidator);
    }
} 