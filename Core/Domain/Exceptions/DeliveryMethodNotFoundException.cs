namespace Domain.Exceptions
{
    public class DeliveryMethodNotFoundException(int id) : NotFoundException($"Delivery method with id {id} was not found!")
    {
    }
}
