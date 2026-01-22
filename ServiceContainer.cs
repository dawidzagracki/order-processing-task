using Microsoft.Extensions.DependencyInjection;

namespace OrderProcessing;

public static class ServiceContainer
{
    public static IServiceProvider Build()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        services.AddSingleton<IOrderValidator, OrderValidator>();
        services.AddSingleton<IOrderService, OrderService>();

        return services.BuildServiceProvider();
    }
}
