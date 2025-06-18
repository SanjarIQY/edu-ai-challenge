# Product Search Application

A console-based product search application that uses OpenAI's function calling capabilities to filter products based on natural language user preferences.

## Features

- **Natural Language Processing**: Enter search queries in plain English
- **OpenAI Function Calling**: Converts natural language to structured filter parameters
- **Clean Architecture**: Implements SOLID principles with proper separation of concerns
- **Configuration Management**: Supports both JSON file and environment variable configuration
- **Flexible Filtering**: Filter by category, price range, rating, stock status, and search terms
- **Rich Product Database**: Products from Electronics, Fitness, Kitchen, Books, Clothing categories

## Prerequisites

- .NET 6.0 or later
- OpenAI API key
- Internet connection for API calls

## Setup

### 1. Navigate to Project Directory
```bash
cd 10.18.06/ProductSearchApp
```

### 2. Install Dependencies
```bash
dotnet restore
```

### 3. Configure OpenAI API Key

**⚠️ SECURITY WARNING: Never commit your actual API key to version control!**

Choose one of these configuration methods:

#### Option A: Using appsettings.json (Recommended for local development)

1. Copy the template file:
```bash
cp appsettings.template.json appsettings.json
```

2. Edit `appsettings.json` and replace the placeholder:
```json
{
  "OpenAI": {
    "ApiKey": "your-actual-api-key-here"
  }
}
```

The `appsettings.json` file is automatically excluded from version control via `.gitignore`.

#### Option B: Using Environment Variables (Recommended for production)

**Temporary (current terminal session only):**
```bash
export OPENAI_API_KEY="your-api-key-here"
```

**Permanent (add to shell profile):**

For Bash (~/.bashrc or ~/.bash_profile):
```bash
echo 'export OPENAI_API_KEY="your-api-key-here"' >> ~/.bashrc
source ~/.bashrc
```

For Zsh (~/.zshrc):
```bash
echo 'export OPENAI_API_KEY="your-api-key-here"' >> ~/.zshrc
source ~/.zshrc
```

### 4. Verify Configuration

The application will automatically check for the API key in this order:
1. `appsettings.json` file
2. `OPENAI_API_KEY` environment variable

### 5. Run Application
```bash
dotnet run
```

## Usage Examples

- "I need a smartphone under $800"
- "Show me fitness products with rating above 4.5"
- "Find kitchen appliances under $100 that are in stock"

## Configuration Priority

The application uses this configuration priority order:
1. **appsettings.json** - Local development configuration
2. **Environment Variables** - System/deployment configuration

This allows for flexible deployment scenarios while keeping sensitive data secure.

## Project Structure

```
ProductSearchApp/
├── Models/
│   ├── Product.cs              # Product data model
│   ├── SearchCriteria.cs       # Search filter model
│   └── AppSettings.cs          # Configuration model
├── Services/                   # Business logic layer
├── Application/                # Orchestration layer
├── Program.cs                  # Entry point with configuration
├── appsettings.template.json   # Template (safe to commit)
├── appsettings.json           # Your config (git ignored)
├── .gitignore                 # Excludes sensitive files
└── products.json              # Product database
```

## Architecture

The application follows clean architecture principles:

- **Models**: Data structures and configuration models
- **Services**: Business logic with dependency injection
- **Application**: Orchestration layer
- **Program**: Entry point with configuration management

## Security Best Practices

✅ **What we do:**
- Exclude `appsettings.json` from version control
- Provide safe template files
- Support environment variables for production
- Clear error messages for missing configuration

❌ **What to avoid:**
- Never commit actual API keys
- Don't hardcode secrets in source code
- Don't share configuration files with API keys

## Troubleshooting

**API Key Issues**: The app will show clear instructions if the API key is missing
**File Not Found**: Ensure you're running from the ProductSearchApp directory
**Network Errors**: Check internet connection and API key validity 