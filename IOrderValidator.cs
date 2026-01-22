namespace OrderProcessing;

public interface IOrderValidator
{
    bool IsValid(int orderId);
}
