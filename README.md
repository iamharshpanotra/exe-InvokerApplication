# Folder Monitor and Application Invoker

This is a simple .NET Core console application that monitors a specified folder for newly created files and automatically launches a configured application with the new file's path as an argument.

## Features

- Monitors a specified directory for new file creation.
- Filters monitoring based on file extensions (optional).
- Launches an external application with the new file as an input argument.
- Configuration is managed through an `AppSettings.json` file.

## Prerequisites

- .NET SDK installed (e.g., .NET Core 3.1 or later)
- Visual Studio or any C# IDE
- A valid external application that accepts a file path as a command-line argument

## Configuration

The application uses an `AppSettings.json` file for runtime configuration. Below is a sample structure:

```json
{
  "AppSettings": {
    "FolderPathToMonitor": "C:\\Path\\To\\Monitor",
    "ApplicationPathToInvoke": "C:\\Path\\To\\YourApp.exe",
    "MonitorAllFiles": "1",
    "MonitorFileExtension": ".txt"
  }
}

