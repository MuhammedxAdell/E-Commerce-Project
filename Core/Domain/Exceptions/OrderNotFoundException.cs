namespace Domain.Exceptions
{
    public class OrderNotFoundException(Guid id) : NotFoundException($"Order with Id: {id} was not found!")
    {
    }
}
