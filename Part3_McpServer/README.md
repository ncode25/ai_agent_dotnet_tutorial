# Part 3: Multi-Client/Server Agent

This final part of the tutorial demonstrates how to build a more robust and scalable AI agent using a client/server architecture. You will create a central agent server that hosts the Semantic Kernel and a separate client application that connects to it.

## Goal

The goal is to create a system where the AI agent runs as a background service (the server), and one or more users can interact with it through a lightweight client application.

## What You'll Learn

- How to build a .NET server application to host a Semantic Kernel agent.
- How to create a .NET client application to communicate with the agent server.
- The fundamentals of building a multi-component application with dependencies.
- A practical architecture for deploying AI agents in a real-world scenario.

## How to Run This Project

This part consists of two separate projects: `Mcp.Server.Local` and `Mcp.Client.Local`. You need to run the server first, and then the client.

### 1. Run the Server

The server application hosts the agent and exposes it for clients to connect.

**Using the .NET CLI:**

1.  Open a terminal and navigate to the server directory:
    ```bash
    cd Part3_McpServer/Mcp.Server.Local
    ```
2.  Run the server:
    ```bash
    dotnet run
    ```

**Using Visual Studio or Rider:**

1.  Open the `Part3_McpServer.sln` file.
2.  Set `Mcp.Server.Local` as the startup project.
3.  Run the project. A console window will appear, indicating that the server is running and waiting for client connections.

### 2. Run the Client

Once the server is running, you can start one or more client applications to interact with the agent.

**Using the .NET CLI:**

1.  Open a **new** terminal and navigate to the client directory:
    ```bash
    cd Part3_McpServer/Mcp.Client.Local
    ```
2.  Run the client:
    ```bash
    dotnet run
    ```

**Using Visual Studio or Rider:**

1.  With the solution open, right-click the `Mcp.Client.Local` project and select "Debug" -> "Start New Instance". This will run the client while keeping the server running.
2.  Alternatively, you can configure the solution to start multiple projects at once.

### 3. Interact with the Agent

A new console window will open for each client instance. You can type questions and get responses from the central agent server. This setup allows multiple users to chat with the same agent simultaneously.

## Project Structure

- **`Mcp.Server.Local/`**: The server project.
  - **`Program.cs`**: Sets up and runs the agent server.
  - **`MenuPlugin.cs`**: The plugin containing the menu knowledge, same as in the previous parts.
- **`Mcp.Client.Local/`**: The client project.
  - **`Program.cs`**: Connects to the server and handles the user chat loop.
- **`Part3_McpServer.sln`**: The solution file that includes both the server and client projects.

Congratulations on completing the tutorial! You have progressed from a simple, local AI agent to a more complex and scalable client/server implementation. 