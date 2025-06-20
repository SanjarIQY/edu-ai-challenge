# Service Analyzer

A console application that generates comprehensive business and technology reports for services using OpenAI's GPT models.

## Prerequisites

- .NET 9.0 SDK or later
- OpenAI API key

## Setup Instructions

1. **Clone the repository**
   ```bash
   git clone <your-repo-url>
   cd edu-ai-challenge/09.16.06/temp-project
   ```

2. **Configure API Key**

   **Option A: Using Configuration File (Recommended for beginners)**
   - Copy `appsettings.json.example` to `appsettings.json`
   - Replace `"your-openai-api-key-here"` with your actual OpenAI API key
   ```json
   {
     "OpenAI": {
       "ApiKey": "your-actual-api-key-here"
     }
   }
   ```

   **Option B: Using Environment Variable (Recommended for deployment)**
   ```bash
   # macOS/Linux
   export OPENAI_API_KEY="your-actual-api-key-here"
   
   # Windows Command Prompt
   set OPENAI_API_KEY=your-actual-api-key-here
   
   # Windows PowerShell
   $env:OPENAI_API_KEY="your-actual-api-key-here"
   ```

3. **Install Dependencies**
   ```bash
   dotnet restore
   ```

4. **Build the Application**
   ```bash
   dotnet build
   ```

## How to Run

### Start the Application
```bash
dotnet run
```

The application will start in interactive mode and display:
```
=== Service Analyzer ===
Analyze services and get comprehensive business reports!
Type 'exit' or 'quit' to close the application.

Enter a service name or description:
```

### Usage Examples
Once running, you can analyze multiple services:
```
Enter a service name or description: Notion
# ... generates report for Notion ...

Enter a service name or description: Slack
# ... generates report for Slack ...

Enter a service name or description: A collaborative note-taking application
# ... generates report for the described service ...

Enter a service name or description: exit
Thank you for using Service Analyzer! Goodbye!
```

## Output Format

The application generates a markdown-formatted report with the following sections:
- **Brief History**: Founding details and milestones
- **Target Audience**: Primary user segments
- **Core Features**: Key functionalities
- **Unique Selling Points**: Competitive advantages
- **Business Model**: Revenue generation methods
- **Tech Stack Insights**: Technology used
- **Perceived Strengths**: Positive aspects
- **Perceived Weaknesses**: Common criticisms

## Troubleshooting

### Common Issues

1. **API Key Error**
   - Ensure your OpenAI API key is valid and has sufficient credits
   - Check that the key is properly set (either in `appsettings.json` or environment variable)

2. **Model Access Error**
   - The application uses `gpt-4.1-mini` by default
   - If you have access to other GPT models, you can change the model in `Program.cs` line 57

3. **Build Errors**
   - Ensure you're using .NET 9.0 SDK
   - Avoid directory paths with spaces or special characters

## Requirements

- OpenAI API key with access to chat models
- Internet connection for API calls
- .NET 9.0 Runtime