# Validation Library

A robust validation library for C# that provides type-safe validation for primitive and complex data structures.

## Features

- Type-safe validation for primitive types (string, number, boolean, date)
- Support for complex types (objects and arrays)
- Fluent API for building validation rules
- Custom error messages
- Comprehensive test coverage

## Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/validation-library.git
```

2. Add the project to your solution:
```bash
dotnet add reference path/to/ValidationLibrary.csproj
```

## Usage

### Basic Validation

```csharp
// String validation
var stringValidator = Schema.String()
    .MinLength(3)
    .MaxLength(50)
    .Pattern(@"^[a-zA-Z]+$");

var result = stringValidator.Validate("John");
if (!result.IsValid)
{
    Console.WriteLine(result.ErrorMessage);
}

// Number validation
var numberValidator = Schema.Number()
    .Min(18)
    .Max(100)
    .Integer();

var result = numberValidator.Validate(25);
if (!result.IsValid)
{
    Console.WriteLine(result.ErrorMessage);
}

// Boolean validation
var boolValidator = Schema.Boolean()
    .Required(true);

var result = boolValidator.Validate(true);
if (!result.IsValid)
{
    Console.WriteLine(result.ErrorMessage);
}

// Date validation
var dateValidator = Schema.Date()
    .Min(DateTime.Now)
    .Max(DateTime.Now.AddYears(1));

var result = dateValidator.Validate(DateTime.Now.AddMonths(6));
if (!result.IsValid)
{
    Console.WriteLine(result.ErrorMessage);
}
```

### Complex Object Validation

```csharp
public class User
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public List<string> Tags { get; set; }
    public Address Address { get; set; }
}

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
}

// Create validators
var addressValidator = Schema.Object<Address>(new Dictionary<string, IValidator<object>>
{
    { "Street", Schema.String().MinLength(5) },
    { "City", Schema.String().MinLength(2) },
    { "PostalCode", Schema.String().Pattern(@"^\d{5}$") }
});

var userValidator = Schema.Object<User>(new Dictionary<string, IValidator<object>>
{
    { "Name", Schema.String().MinLength(2).MaxLength(50) },
    { "Age", Schema.Number().Min(18) },
    { "Email", Schema.String().Pattern(@"^[^\s@]+@[^\s@]+\.[^\s@]+$") },
    { "Tags", Schema.Array(Schema.String()).MinLength(1) },
    { "Address", addressValidator }
});

// Validate user
var user = new User
{
    Name = "John Doe",
    Age = 25,
    Email = "john@example.com",
    Tags = new List<string> { "developer", "designer" },
    Address = new Address
    {
        Street = "123 Main St",
        City = "Anytown",
        PostalCode = "12345"
    }
};

var result = userValidator.Validate(user);
if (!result.IsValid)
{
    Console.WriteLine(result.ErrorMessage);
}
```

### Custom Error Messages

```csharp
var validator = Schema.String()
    .MinLength(5)
    .WithMessage("Name must be at least 5 characters long");

var result = validator.Validate("John");
if (!result.IsValid)
{
    Console.WriteLine(result.ErrorMessage); // Outputs: "Name must be at least 5 characters long"
}
```

## Running Tests

To run the tests:

```bash
dotnet test
```

To generate a test coverage report:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details. 