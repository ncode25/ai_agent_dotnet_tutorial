# .NET AI Agent Tutorial with Semantic Kernel

This repository contains a comprehensive tutorial for building AI-powered agents using .NET and the Microsoft Semantic Kernel. The tutorial is divided into three parts, each building upon the previous one, to provide a step-by-step guide from a simple agent to a more complex multi-client setup.

## Tutorial Structure

### [Part 1: Build a Restaurant Menu Agent](./Part1_Build_Restuarant_Agent/Readme.md)

This part covers the fundamentals of the Semantic Kernel. You will build a simple "Restaurant Menu Agent" that can answer questions about a restaurant's menu.

**Key concepts covered:**
- Setting up Semantic Kernel with OpenAI
- Creating and using plugins
- Interacting with the agent from a console application

### [Part 2: Run LLM Locally](./Part2_Run_LLM_Locally/README.md)

In the second part, you will learn how to switch from a cloud-based LLM service like OpenAI to a locally running model using Ollama. This is useful for offline development, cost savings, and data privacy.

**Key concepts covered:**
- Integrating Semantic Kernel with Ollama
- Running local LLMs like Llama 3
- Modifying the agent to use a local model

### [Part 3: Multi-Client/Server Agent using Model Context Protocol](./Part3_McpServer/README.md) 

The final part demonstrates a more advanced architecture where a central agent server manages the core logic and multiple clients can connect to it.

**Key concepts covered:**
- Building a server for the Semantic Kernel agent
- Creating a client application to interact with the agent server
- Real-world application architecture for AI agents

## Prerequisites

Before you begin, ensure you have the following installed:
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/)
- An [OpenAI API key](https://platform.openai.com/account/api-keys) (for Part 1)
- [Ollama](https://ollama.com/) (for Part 2)

## How to Use This Tutorial

1.  **Start with Part 1:** It is highly recommended to follow the tutorial in order.
2.  **Navigate to each part's directory:** Each part is a self-contained .NET project.
3.  **Follow the instructions:** Each part has its own `README.md` file with detailed instructions on how to run the project.

Happy coding! 