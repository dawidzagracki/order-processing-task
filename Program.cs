using Microsoft.Extensions.DependencyInjection;
using OrderProcessing;

var serviceProvider = ServiceContainer.Build();
var orderService = serviceProvider.GetRequiredService<IOrderService>();
var repository = serviceProvider.GetRequiredService<IOrderRepository>();

async Task ProcessOrder(int orderId)
{
    await orderService.ProcessOrderAsync(orderId);
}

var tasks = new[]
{
    ProcessOrder(1),
    ProcessOrder(2),
    ProcessOrder(-1),
    Task.Run(async () =>
    {
        await repository.AddOrderAsync(new Order { Id = 3, ProductName = "Tablet" });
    })
};

await Task.WhenAll(tasks);

Console.WriteLine("All orders processed.");
