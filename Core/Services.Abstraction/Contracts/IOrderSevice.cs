using Shared.Dtos.OrderModule;

namespace Services.Abstraction.Contracts
{
    public interface IOrderSevice
    {
        //GrtById ==> Take Guid id ==> return OrderResult
        Task<OrderResult> GetOrderByIdAsync(Guid id);
        //GetAllByEmail ==> Take string Email ==> return IEnumerable<OrderResult>
        Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string userEmail);
        //CreateOrder ==> Take OrderRequest , string Email ==> return OrderResult
        Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string userEmail);
        //GetDeliveryMethods ==> return IEnumerable<DeliveryMethodResult>
        Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync();
    }
}
