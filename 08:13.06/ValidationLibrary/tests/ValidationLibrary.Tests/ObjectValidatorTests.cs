using System.Collections.Generic;
using Xunit;
using ValidationLibrary;

namespace ValidationLibrary.Tests
{
    public class ObjectValidatorTests
    {
        private class TestObject
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public bool IsActive { get; set; }
        }

        [Fact]
        public void Validate_WithNullValue_ReturnsFailure()
        {
            // Arrange
            var schema = new Dictionary<string, IValidator<object>>
            {
                { "Name", Schema.String() }
            };
            var validator = Schema.Object<TestObject>(schema);

            // Act
            var result = validator.Validate(null);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Object cannot be null", result.ErrorMessage);
        }

        [Fact]
        public void Validate_WithInvalidProperty_ReturnsFailure()
        {
            // Arrange
            var schema = new Dictionary<string, IValidator<object>>
            {
                { "Name", Schema.String().MinLength(5) }
            };
            var validator = Schema.Object<TestObject>(schema);
            var obj = new TestObject { Name = "test" };

            // Act
            var result = validator.Validate(obj);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Name: String length must be at least 5 characters", result.ErrorMessage);
        }

        [Fact]
        public void Validate_WithMultipleInvalidProperties_ReturnsAllErrors()
        {
            // Arrange
            var schema = new Dictionary<string, IValidator<object>>
            {
                { "Name", Schema.String().MinLength(5) },
                { "Age", Schema.Number().Min(18) }
            };
            var validator = Schema.Object<TestObject>(schema);
            var obj = new TestObject { Name = "test", Age = 16 };

            // Act
            var result = validator.Validate(obj);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Name: String length must be at least 5 characters", result.ErrorMessage);
            Assert.Contains("Age: Value must be at least 18", result.ErrorMessage);
        }

        [Fact]
        public void Validate_WithCustomMessage_ReturnsCustomMessage()
        {
            // Arrange
            var schema = new Dictionary<string, IValidator<object>>
            {
                { "Name", Schema.String().MinLength(5) }
            };
            var validator = Schema.Object<TestObject>(schema)
                .WithMessage("Custom error message");
            var obj = new TestObject { Name = "test" };

            // Act
            var result = validator.Validate(obj);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Custom error message", result.ErrorMessage);
        }

        [Fact]
        public void Validate_WithValidObject_ReturnsSuccess()
        {
            // Arrange
            var schema = new Dictionary<string, IValidator<object>>
            {
                { "Name", Schema.String().MinLength(3) },
                { "Age", Schema.Number().Min(18) },
                { "IsActive", Schema.Boolean() }
            };
            var validator = Schema.Object<TestObject>(schema);
            var obj = new TestObject { Name = "John", Age = 25, IsActive = true };

            // Act
            var result = validator.Validate(obj);

            // Assert
            Assert.True(result.IsValid);
            Assert.Null(result.ErrorMessage);
        }
    }
} 