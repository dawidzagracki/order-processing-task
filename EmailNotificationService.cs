namespace OrderProcessing;

public class EmailNotificationService : INotificationService
{
    public Task SendAsync(string message)
    {
        Console.WriteLine($"[Email Notification] {message}");
        return Task.CompletedTask;
    }
}
