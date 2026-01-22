namespace OrderProcessing;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly ILogger _logger;
    private readonly IOrderValidator _validator;
    private readonly INotificationService _notificationService;

    public OrderService(
        IOrderRepository repository,
        ILogger logger,
        IOrderValidator validator,
        INotificationService notificationService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }

    public async Task ProcessOrderAsync(int orderId)
    {
        _logger.LogInfo($"Starting to process order {orderId}.");

        if (!_validator.IsValid(orderId))
        {
            _logger.LogError($"Invalid order ID: {orderId}.", new ArgumentException("Order ID must be positive."));
            return;
        }

        try
        {
            var productName = await _repository.GetOrderAsync(orderId);
            _logger.LogInfo($"Order {orderId} processed successfully. Product: {productName}");
            await _notificationService.SendAsync($"Order {orderId} ({productName}) has been processed.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to process order {orderId}.", ex);
        }
    }
}
