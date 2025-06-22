using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Configuration;

public class KernelServiceLLM
{
    public readonly Kernel _kernel;
    private readonly MenuPlugin _menuPlugin;
    private readonly string _modelId;


    public KernelServiceLLM(IConfiguration configuration)
    {
        _modelId = configuration["Ollama:ModelId"] ?? "llama3:latest";
        var builder = Kernel.CreateBuilder();
        var handler = GetHttpClientHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost:11434"), // update to your local port
            Timeout = TimeSpan.FromSeconds(300) // Set timeout to 5 minutes becuase local LLMs can take longer to respond
        };

        builder.AddOllamaChatCompletion(_modelId, httpClient);

        builder.Plugins.AddFromType<MenuPlugin>();
        _kernel = builder.Build();
        _menuPlugin = new MenuPlugin();
    }
    /// <summary>
    /// Get chat completion response asynchronously, handling both GPT and non-GPT models.
    /// </summary>
    /// <param name="chatHistory"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<StreamingChatMessageContent> GetChatCompletionResponseAsync(
       ChatHistory chatHistory,
       OpenAIPromptExecutionSettings settings)
    {

        await foreach (var chatUpdate in GetStreamingChatMessageContentsAsync(chatHistory))
            {
                yield return chatUpdate;
            }
    }


    /// <summary>
    /// Custom method to get streaming chat message contents using the menu plugin for non-GPT models.
    /// We neeed this method to work around the fact that Ollama does not support function calling yet.
    /// </summary>
    /// <param name="chatHistory"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
        ChatHistory chatHistory)
    {
        var prompt = """
        You are a helpful restaurant assistant. Use the menu knowledge below to only answer menu related customer questions.

        {{$kb}}

        User: {{$userInput}}
        Assistant:

        If the user asks a question that is not related to the knowledge base, be graceful and polite and let them know you are not able to answer that question.
        Do not make up answers. If the answer is not in the knowledge base, say you don't know.
        Extract the information and answer the question. If the answer is not found, say you don't know.
        """;

        var promptFunction = _kernel.CreateFunctionFromPrompt(prompt);


        await foreach (var chatUpdate in _kernel.InvokeStreamingAsync(promptFunction, new()
        {
            ["kb"] = _menuPlugin.GetRestaurantMenu(),
            ["userInput"] = chatHistory.Last().Content
        }))

        {
            yield return (StreamingChatMessageContent)chatUpdate;
        }
    }

    private HttpClientHandler GetHttpClientHandler()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            if (errors == System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors)
            {
                return IsOnlyRevocationError(chain);
            }
            return errors == System.Net.Security.SslPolicyErrors.None;
        };
        return handler;
    }

    private bool IsOnlyRevocationError(System.Security.Cryptography.X509Certificates.X509Chain? chain)
    {
        if (chain == null) return true;

        foreach (var status in chain.ChainStatus)
        {
            if (!IsRevocationStatus(status.Status))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsRevocationStatus(System.Security.Cryptography.X509Certificates.X509ChainStatusFlags status)
    {
        return status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.RevocationStatusUnknown ||
               status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.OfflineRevocation;
    }
}