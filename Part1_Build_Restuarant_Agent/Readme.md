# Part 1: Build a Restaurant Agent

Welcome to the first part of the .NET AI Agent tutorial! In this section, you will build a simple but powerful "Restaurant Agent" using the Microsoft Semantic Kernel and OpenAI.

## Goal

The goal of this part is to create a console application where you can chat with an AI agent that has knowledge about a restaurant's menu.

## What You'll Learn

- How to set up a .NET project with the Semantic Kernel SDK.
- How to connect to OpenAI's chat completion API.
- What plugins are and how to create a custom plugin for specific knowledge.
- How to stream responses from the agent.

## How to Run This Project

### 1. Configure Your OpenAI API Key

To run this project, you need an OpenAI API key. The project is set up to read the key from the .NET User Secrets store. This is a secure way to handle sensitive data.

**Set the API key using the .NET CLI:**

1. Open a terminal in this directory (`Part1_Build_Restuarant_Agent`).
2. Initialize user secrets for the project. This only needs to be done once.
    ```bash
    dotnet user-secrets init
    ```
3. Run the following command, replacing `"your_api_key_here"` with your actual OpenAI API key and optionally specifying a `model_id`. If `model_id` is not provided, it will default to `gpt-4o`.

```bash
dotnet user-secrets set "OpenAI:ApiKey" "your_api_key_here"
dotnet user-secrets set "OpenAI:ModelId" "your_model_id"
```

### 2. Run the Application

Once the API key is configured, you can run the project.

**Using the .NET CLI:**

```bash
dotnet run
```

**Using Visual Studio or Rider:**

- Open the `Part1_Build_Restuarant_Agent.sln` file.
- Set `Part1_Build_Restuarant_Agent` as the startup project.
- Press the "Run" button.

### 3. Interact with the Agent

After running the application, a console window will appear. You can start asking questions about the restaurant's menu. For example:

- "What kind of soups do you have?"
- "Tell me about the T-Bone Steak."
- "What is the most expensive item on the menu?"

To exit the application, simply close the console window or press `Ctrl+C`.

## Project Structure

- **`Program.cs`**: The main entry point of the application. It contains the chat loop that reads user input and sends it to the agent.
- **`KernelService.cs`**: This class is responsible for initializing and configuring the Semantic Kernel, including the connection to OpenAI and the registration of plugins.
- **`KnowledgePlugin.cs`**: A custom plugin that provides the agent with specific knowledge about the restaurant's menu. This is where the menu data is stored.
- **`Part1_Build_Restuarant_Agent.csproj`**: The .NET project file, which defines project settings and dependencies.

Now, let's move on to [Part 2](./../Part2_Run_LLM_Locally/README.md), where you will learn how to run a Large Language Model (LLM) locally.