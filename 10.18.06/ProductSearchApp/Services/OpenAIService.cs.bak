using System.Text.Json;
using OpenAI;
using OpenAI.Chat;
using ProductSearchApp.Models;

namespace ProductSearchApp.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly ChatClient _chatClient;
        private const string Model = "gpt-4.1-mini";
        private const string FunctionName = "search_products";

        public OpenAIService(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

            var openAIClient = new OpenAIClient(apiKey);
            _chatClient = openAIClient.GetChatClient(Model);
        }

        public async Task<SearchCriteria?> ParseSearchQueryAsync(string userQuery, List<Product> availableProducts)
        {
            if (string.IsNullOrWhiteSpace(userQuery))
                throw new ArgumentException("User query cannot be null or empty", nameof(userQuery));

            if (availableProducts == null)
                throw new ArgumentNullException(nameof(availableProducts));

            try
            {
                var systemMessage = CreateSystemMessage(availableProducts);
                var messages = new List<ChatMessage>
                {
                    ChatMessage.CreateSystemMessage(systemMessage),
                    ChatMessage.CreateUserMessage(userQuery)
                };

                var chatCompletionOptions = new ChatCompletionOptions
                {
                    Tools = {
                        ChatTool.CreateFunctionTool(
                            functionName: FunctionName,
                            functionDescription: "Search for products based on user preferences and filtering criteria",
                            functionParameters: BinaryData.FromString(GetFunctionParametersSchema())
                        )
                    }
                };

                var chatCompletion = await _chatClient.CompleteChatAsync(messages, chatCompletionOptions);

                return ExtractSearchCriteriaFromResponse(chatCompletion);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to parse search query: {ex.Message}", ex);
            }
        }

        private string CreateSystemMessage(List<Product> availableProducts)
        {
            var categories = availableProducts.Select(p => p.Category).Distinct().ToList();
            var priceRange = GetPriceRange(availableProducts);
            var ratingRange = GetRatingRange(availableProducts);

            return $@"You are a product search assistant. You have access to a product database with these specifications:

Available categories: {string.Join(", ", categories)}
Price range: ${priceRange.Min:F2} - ${priceRange.Max:F2}
Rating range: {ratingRange.Min:F1} - {ratingRange.Max:F1}

When a user makes a search request, analyze their preferences and call the search_products function with appropriate filter criteria.
Extract filtering criteria from natural language and convert to structured parameters.

Available products count: {availableProducts.Count}";
        }

        private (decimal Min, decimal Max) GetPriceRange(List<Product> products)
        {
            if (!products.Any())
                return (0, 0);

            return (products.Min(p => p.Price), products.Max(p => p.Price));
        }

        private (double Min, double Max) GetRatingRange(List<Product> products)
        {
            if (!products.Any())
                return (0, 0);

            return (products.Min(p => p.Rating), products.Max(p => p.Rating));
        }

        private string GetFunctionParametersSchema()
        {
            return @"{
                ""type"": ""object"",
                ""properties"": {
                    ""Category"": {
                        ""type"": ""string"",
                        ""description"": ""Product category to filter by"",
                        ""enum"": [""Electronics"", ""Fitness"", ""Kitchen"", ""Books"", ""Clothing""]
                    },
                    ""MaxPrice"": {
                        ""type"": ""number"",
                        ""description"": ""Maximum price filter""
                    },
                    ""MinPrice"": {
                        ""type"": ""number"",
                        ""description"": ""Minimum price filter""
                    },
                    ""MinRating"": {
                        ""type"": ""number"",
                        ""description"": ""Minimum rating filter""
                    },
                    ""InStockOnly"": {
                        ""type"": ""boolean"",
                        ""description"": ""Filter for only in-stock products""
                    },
                    ""SearchTerm"": {
                        ""type"": ""string"",
                        ""description"": ""Text to search in product names""
                    }
                }
            }";
        }

        private SearchCriteria? ExtractSearchCriteriaFromResponse(ChatCompletion chatCompletion)
        {
            if (chatCompletion.ToolCalls.Count == 0)
                return null;

            var toolCall = chatCompletion.ToolCalls.FirstOrDefault(tc => tc.FunctionName == FunctionName);
            if (toolCall == null)
                return null;

            var functionArguments = toolCall.FunctionArguments.ToString();
            var searchCriteria = JsonSerializer.Deserialize<SearchCriteria>(functionArguments, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return searchCriteria;
        }
    }
} 