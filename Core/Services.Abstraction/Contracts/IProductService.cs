using Shared.Dtos;
using Shared.Enums;

namespace Services.Abstraction.Contracts
{
    public interface IProductService
    {
        //GetAllProductsAsync
        Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(int? typeId , int? brandId , ProductSortingOptions sort);
        //GetAllBrandsAsync
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        //GetAllTypesAsync
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
        //GetProductByIdAsync
        Task<ProductResultDto> GetProductByIdAsync(int id);
    }
}
