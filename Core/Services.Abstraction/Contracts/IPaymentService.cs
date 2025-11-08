using Shared.Dtos.BasketModule;

namespace Services.Abstraction.Contracts
{
    public interface IPaymentService
    {
        Task<BasketDto?> CreateOrUpdatePaymentIntentAsync(string basketId);

        Task UpadatePaymentStatusAsync(string json, string signatureHeader);

    }
}
