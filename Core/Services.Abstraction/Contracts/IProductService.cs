using Shared.Dtos;

namespace Services.Abstraction.Contracts
{
    public interface IProductService
    {
        //GetAllProductsAsync
        Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(int? typeId , int? brandId);
        //GetAllBrandsAsync
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        //GetAllTypesAsync
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
        //GetProductByIdAsync
        Task<ProductResultDto> GetProductByIdAsync(int id);
    }
}
