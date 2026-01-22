using System.Collections.Concurrent;

namespace OrderProcessing;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<int, Order> _orders;

    public InMemoryOrderRepository()
    {
        _orders = new ConcurrentDictionary<int, Order>();
        _orders.TryAdd(1, new Order { Id = 1, ProductName = "Laptop" });
        _orders.TryAdd(2, new Order { Id = 2, ProductName = "Phone" });
    }

    public async Task<string> GetOrderAsync(int orderId)
    {
        if (orderId <= 0)
            throw new ArgumentException("Order ID must be positive.", nameof(orderId));

        await Task.Delay(100);

        if (!_orders.TryGetValue(orderId, out var order))
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");

        return order.ProductName;
    }

    public Task AddOrderAsync(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        if (!_orders.TryAdd(order.Id, order))
            throw new InvalidOperationException($"Order with ID {order.Id} already exists.");

        return Task.CompletedTask;
    }
}
