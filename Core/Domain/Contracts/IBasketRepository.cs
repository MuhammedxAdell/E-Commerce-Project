using Domain.Entities.BasketModule;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        //Get Basket by Id
        Task<CustomerBasket?> GetBasketAsync(string basketId);
        //Update or Create Basket
        Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket , TimeSpan? timeToLive = null);
        //Delete Basket
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
