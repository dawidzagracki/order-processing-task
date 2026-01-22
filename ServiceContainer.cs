using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderProcessing;

public static class ServiceContainer
{
    public static IServiceProvider Build()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<ILogger, ConsoleLogger>();
        services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        services.AddSingleton<IOrderValidator, OrderValidator>();
        services.AddSingleton<INotificationService, EmailNotificationService>();
        services.AddSingleton<IOrderService, OrderService>();

        return services.BuildServiceProvider();
    }
}
