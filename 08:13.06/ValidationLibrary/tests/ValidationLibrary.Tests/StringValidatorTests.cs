using Xunit;
using ValidationLibrary;

namespace ValidationLibrary.Tests
{
    public class StringValidatorTests
    {
        [Fact]
        public void Validate_WithNullValue_ReturnsFailure()
        {
            // Arrange
            var validator = Schema.String();

            // Act
            var result = validator.Validate(null);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Value cannot be null", result.ErrorMessage);
        }

        [Fact]
        public void Validate_WithMinLength_WhenTooShort_ReturnsFailure()
        {
            // Arrange
            var validator = Schema.String().MinLength(5);

            // Act
            var result = validator.Validate("test");

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("String length must be at least 5 characters", result.ErrorMessage);
        }

        [Fact]
        public void Validate_WithMaxLength_WhenTooLong_ReturnsFailure()
        {
            // Arrange
            var validator = Schema.String().MaxLength(5);

            // Act
            var result = validator.Validate("too long string");

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("String length must not exceed 5 characters", result.ErrorMessage);
        }

        [Fact]
        public void Validate_WithPattern_WhenNoMatch_ReturnsFailure()
        {
            // Arrange
            var validator = Schema.String().Pattern(@"^\d+$");

            // Act
            var result = validator.Validate("abc");

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("String does not match required pattern", result.ErrorMessage);
        }

        [Fact]
        public void Validate_WithCustomMessage_ReturnsCustomMessage()
        {
            // Arrange
            var validator = Schema.String().MinLength(5).WithMessage("Custom error message");

            // Act
            var result = validator.Validate("test");

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Custom error message", result.ErrorMessage);
        }

        [Fact]
        public void Validate_WithValidString_ReturnsSuccess()
        {
            // Arrange
            var validator = Schema.String()
                .MinLength(3)
                .MaxLength(10)
                .Pattern(@"^[a-z]+$");

            // Act
            var result = validator.Validate("valid");

            // Assert
            Assert.True(result.IsValid);
            Assert.Null(result.ErrorMessage);
        }
    }
} 