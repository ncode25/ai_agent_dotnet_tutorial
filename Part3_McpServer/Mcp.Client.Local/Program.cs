using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mcp.Client.Local
{
#pragma warning disable
    internal class Program
    {
        static async Task Main()
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddUserSecrets<Program>();
            var config = builder.Build().Services.GetRequiredService<IConfiguration>();

            var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
            {
                Name = "Local-MCP-Server",
                Command = "dotnet",
                Arguments =
                [
                    "run",
                    "--project",
                    "/Users/nishchal.jain/Documents/dev/Learn/ai_agent_dotnet_tutorial/Part3_McpServer/Mcp.Server.Local",
                    "--no-build"
                ],
            });

            var mcpClient = await McpClientFactory.CreateAsync(clientTransport);

            var tools = await mcpClient.ListToolsAsync().ConfigureAwait(false);

            foreach (var tool in tools)
            {
                Console.WriteLine($"{tool.Name}: {tool.Description}");
            }

            var kernel = CreateBuilder(config);

            kernel.Plugins.AddFromFunctions("LocalMCPServer",
                tools.Select(aiFunction => aiFunction.AsKernelFunction()));

            OpenAIPromptExecutionSettings executionSettings = new()
            {
                Temperature = 0,
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(options: new() { RetainArgumentTypes = true })
            };

            var prompt = "list all vegan dishes";
            var result = await kernel.InvokePromptAsync(prompt, new(executionSettings)).ConfigureAwait(false);
            Console.WriteLine($"\n\n{prompt}\n{result}");

            Console.Read();
        }


        private static Kernel CreateBuilder(IConfiguration config)
        {
            var modelId = config["OpenAI:ModelId"] ?? "gpt-4o";
            var apiKey = config["OpenAI:ApiKey"]; // only required if you are using cloud based Open AI LLM
            var modelId_Local = config["Ollama:ModelId"] ?? "mistral:latest";


            var builder = Kernel.CreateBuilder();
            var handler = GetHttpClientHandler();

            // Uncomment below lines if you want to use Open AI chat completion. Comment OllamaChatCompletion
            // var httpclient = new HttpClient(handler);
            // builder.AddOpenAIChatCompletion(modelId: modelId, apiKey: apiKey, httpClient: httpclient);

            
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost:11434"),
                Timeout = TimeSpan.FromSeconds(300) // Set timeout to 5 minutes becuase local LLMs can take longer to respond
            };
            builder.AddOllamaChatCompletion(modelId_Local, httpClient);

            Kernel kernel = builder.Build();
            return kernel;
        }

        private static HttpClientHandler GetHttpClientHandler()
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
}