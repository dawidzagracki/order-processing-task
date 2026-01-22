namespace OrderProcessing;

public interface INotificationService
{
    Task SendAsync(string message);
}
