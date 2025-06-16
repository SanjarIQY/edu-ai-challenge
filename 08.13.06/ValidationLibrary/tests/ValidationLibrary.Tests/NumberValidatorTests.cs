using Xunit;
using ValidationLibrary;

namespace ValidationLibrary.Tests
{
    public class NumberValidatorTests
    {
        [Fact]
        public void Validate_WithMinValue_WhenTooSmall_ReturnsFailure()
        {
            // Arrange
            var validator = Schema.Number().Min(5);

            // Act
            var result = validator.Validate(3);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Value must be at least 5", result.ErrorMessage);
        }

        [Fact]
        public void Validate_WithMaxValue_WhenTooLarge_ReturnsFailure()
        {
            // Arrange
            var validator = Schema.Number().Max(5);

            // Act
            var result = validator.Validate(7);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Value must not exceed 5", result.ErrorMessage);
        }

        [Fact]
        public void Validate_WithInteger_WhenDecimal_ReturnsFailure()
        {
            // Arrange
            var validator = Schema.Number().Integer();

            // Act
            var result = validator.Validate(3.14);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Value must be an integer", result.ErrorMessage);
        }

        [Fact]
        public void Validate_WithCustomMessage_ReturnsCustomMessage()
        {
            // Arrange
            var validator = Schema.Number().Min(5).WithMessage("Custom error message");

            // Act
            var result = validator.Validate(3);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Custom error message", result.ErrorMessage);
        }

        [Fact]
        public void Validate_WithValidNumber_ReturnsSuccess()
        {
            // Arrange
            var validator = Schema.Number()
                .Min(3)
                .Max(10)
                .Integer();

            // Act
            var result = validator.Validate(5);

            // Assert
            Assert.True(result.IsValid);
            Assert.Null(result.ErrorMessage);
        }
    }
} 