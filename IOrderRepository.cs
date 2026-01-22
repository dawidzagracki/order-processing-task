namespace OrderProcessing;

public interface IOrderRepository
{
    Task<string> GetOrderAsync(int orderId);
    Task AddOrderAsync(Order order);
}
