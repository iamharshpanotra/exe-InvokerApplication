using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main()
    {
        // Set up the configuration by reading values from the AppSettings.json file.
        // This allows us to use values from the configuration instead of hard-coding them.
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Set the base directory to the current directory of the application.
            .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true) // Read the appsettings.json file
            .Build();

        // Read configuration values from AppSettings.json.
        string folderPath = config["AppSettings:FolderPathToMonitor"];  // The path of the folder to monitor.
        string applicationPath = config["AppSettings:ApplicationPathToInvoke"];  // The path of the application to invoke.
        bool monitorAllFiles = config["AppSettings:MonitorAllFiles"] == "1";  // Flag to monitor all files or filter by extension.
        string monitorFileExtension = config["AppSettings:MonitorFileExtension"];  // Extension to monitor if not monitoring all files.


        // Validate that the folder path is not empty and exists.
        if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
        {
            Console.WriteLine("Error: The folder path does not exist or is empty.");
            return;   // Exit if the folder path is invalid.
        }

        // Set the filter for the file system watcher based on whether all files should be monitored or only specific extensions.
        string filter = monitorAllFiles ? "*.*" : $"*{monitorFileExtension}";

        // Create and configure the FileSystemWatcher to monitor the folder.
        FileSystemWatcher watcher = new FileSystemWatcher
        {
            Path = folderPath,   // Set the folder path to monitor.
            Filter = filter,    // Set the filter to monitor all files or only the specified extension.
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime   // Watch for changes in file name or creation time.
        };


        // Event handler that gets triggered when a new file is created in the monitored folder.
        watcher.Created += (sender, e) =>
        {
            Console.WriteLine($"New file detected: {e.FullPath}");    // Output the path of the new file.
            StartApplication(applicationPath, e.FullPath);    // Invoke the specified application with the new file path as an argument.
        };


        // Start monitoring the folder for changes.
        watcher.EnableRaisingEvents = true;


        // Inform the user that the folder is being monitored.
        Console.WriteLine($"Monitoring folder: {folderPath}");
        Console.WriteLine("Press [Enter] to exit.");
        Console.ReadLine();  // Wait for user input to exit.


        // Validate that the application path is not empty and the application file exists.
        if (string.IsNullOrEmpty(applicationPath) || !File.Exists(applicationPath))
        {
            Console.WriteLine("Error: The application file does not exist or is invalid.");
            return;  // Exit if the application file is invalid.
        }
    }


    // Method to start the application with the specified path and argument.
    static void StartApplication(string appPath, string argument)
    {
        try
        {
            // Start the application with the argument passed to it (the newly created file).
            Process.Start(appPath, $"\"{argument}\"");
            Console.WriteLine($"Application invoked: {appPath} with argument: {argument}");
        }
        catch (Exception ex)
        {
            // Catch and display any errors that occur when trying to start the application.
            Console.WriteLine($"Error starting application: {ex.Message}");
        }
    }
}
