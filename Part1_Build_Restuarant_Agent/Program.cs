// See https://aka.ms/new-console-template for more information
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

Console.WriteLine("Welcome to the Restaurant Menu Agent Builder!");
Console.WriteLine("This application will help you create a simple restaurant agent using Semantic Kernel in .NET.");

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddSingleton<KernelService>(); 

var app = builder.Build();

using var scope = app.Services.CreateScope();
var kernelService = scope.ServiceProvider.GetRequiredService<KernelService>();

var chatHistory = new ChatHistory(new List<ChatMessageContent>
{
    new ChatMessageContent(AuthorRole.System, $"The current date/time in UTC is {DateTime.UtcNow}. You are a helpful restaurant menu assistant.")
});

while (true)
{
    Console.Write("\nAsk about the restaurant menu (or type 'exit' to quit): ");
    var line = Console.ReadLine();
    if (line == "exit")
    {
        break;
    }

    chatHistory.Add(new ChatMessageContent(AuthorRole.User, line));

    // Streaming response
    var streamedResponse = new StringBuilder();
    await foreach (var chatUpdate in kernelService.GetChatCompletionResponseAsync(
        chatHistory,
        new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        }))
    {
        Console.Write(chatUpdate.Content);
        streamedResponse.Append(chatUpdate.Content);
    }
    Console.WriteLine();

    // Add the full streamed response to chat history
    var chatResponse = new ChatMessageContent(AuthorRole.Assistant, streamedResponse.ToString());
    chatHistory.Add(chatResponse);
}