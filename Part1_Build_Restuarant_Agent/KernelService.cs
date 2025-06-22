using Microsoft.SemanticKernel;
using Microsoft.Extensions.Logging;
using Part1_Build_Restuarant_Agent;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

public class KernelService
{
    public readonly Kernel _kernel;
    private const string ModelId = "gpt-4o";
    private const string ApiKey = "your_api_key_here";

    public KernelService()
    {
        var builder = Kernel.CreateBuilder();
        var handler = GetHttpClientHandler();
        var httpclient = new HttpClient(handler);
        builder.AddOpenAIChatCompletion(modelId: ModelId, apiKey: ApiKey, httpClient: httpclient);
        builder.Plugins.AddFromType<KnowledgePlugin>();
        _kernel = builder.Build();
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetChatCompletionResponseAsync(
       ChatHistory chatHistory,
       OpenAIPromptExecutionSettings settings)
    {
        var chatClient = _kernel.GetRequiredService<IChatCompletionService>();
        await foreach (var chatUpdate in chatClient.GetStreamingChatMessageContentsAsync(
            chatHistory,
            settings,
            _kernel
        ))
        {
            yield return chatUpdate;
        }
    }


    private HttpClientHandler GetHttpClientHandler()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            // Ignore only revocation errors
            if (errors == System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors)
            {
                // Add null check for chain to prevent CS8602
                if (chain != null)
                {
                    foreach (var status in chain.ChainStatus)
                    {
                        if (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.RevocationStatusUnknown ||
                            status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.OfflineRevocation)
                        {
                            continue; // ignore revocation errors
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return errors == System.Net.Security.SslPolicyErrors.None;
        };

        return handler;
    }    
}