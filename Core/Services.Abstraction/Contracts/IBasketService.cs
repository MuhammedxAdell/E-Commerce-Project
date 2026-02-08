using Shared.Dtos.BasketModule;

namespace Services.Abstraction.Contracts
{
    public interface IBasketService
    {
        //Get
        Task<BasketDto> GetBasketAsync(string basketId);
        //Dlete 
        Task<bool> DeleteBasketAsync(string basketId);
        //CreateOrUpdate
        Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDto);

    }
}
