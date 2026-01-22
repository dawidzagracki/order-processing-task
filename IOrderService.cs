namespace OrderProcessing;

public interface IOrderService
{
    Task ProcessOrderAsync(int orderId);
}
