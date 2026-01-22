namespace OrderProcessing;

public class OrderValidator : IOrderValidator
{
    public bool IsValid(int orderId)
    {
        return orderId > 0;
    }
}
