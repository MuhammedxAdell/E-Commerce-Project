using Shared;
using Shared.Dtos.ProductModule;
using Shared.Enums;

namespace Services.Abstraction.Contracts
{
    public interface IProductService
    {
        //GetAllProductsAsync
        Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParameters parameters);
        //GetAllBrandsAsync
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        //GetAllTypesAsync
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
        //GetProductByIdAsync
        Task<ProductResultDto> GetProductByIdAsync(int id);
    }
}
