using Microsoft.Extensions.Configuration;

namespace OrderProcessing;

public class ConsoleLogger : ILogger
{
    private readonly string _logLevel;

    public ConsoleLogger(IConfiguration configuration)
    {
        _logLevel = configuration["LogLevel"] ?? "Info";
    }

    public void LogInfo(string message)
    {
        if (_logLevel == "Error")
            return;

        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Console.WriteLine($"[{timestamp}] INFO: {message}");
    }

    public void LogError(string message, Exception ex)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Console.WriteLine($"[{timestamp}] ERROR: {message}");
        Console.WriteLine($"Exception: {ex.GetType().Name} - {ex.Message}");
    }
}
