namespace OrderProcessing;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IOrderValidator _validator;

    public OrderService(
        IOrderRepository repository,
        IOrderValidator validator)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task ProcessOrderAsync(int orderId)
    {
        if (!_validator.IsValid(orderId))
        {
            return;
        }

        try
        {
            await _repository.GetOrderAsync(orderId);
        }
        catch (Exception)
        {
        }
    }
}
