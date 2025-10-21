using AutoMapper;
using Domain.Contracts;
using Domain.Entities.ProductModule;
using Services.Abstraction.Contracts;
using Services.Specifications;
using Shared.Dtos;

namespace Services.Implementations
{
    public class ProductService(IUnitOfWork _unitOfWork , IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            //1- UnitOfWork ==> GenericRepository ==> GetAllBrandsAsync() ==> IEnumerable<ProductBrand>
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            //2- AutoMapper ==> IEnumerable<ProductBrand> ==> IEnumerable<BrandResultDto>
            var brandResult = _mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return brandResult;
        }

        public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(int? typeId , int? brandId)
        {
            var specifications = new ProductWithBrandAndTypeSpecifictaions(typeId , brandId);
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(specifications);
            var productResult = _mapper.Map<IEnumerable<ProductResultDto>>(products);
            return productResult;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await  _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var typeResult = _mapper.Map<IEnumerable<TypeResultDto>>(types);
            return typeResult;
        }

        public async Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            var specifications = new ProductWithBrandAndTypeSpecifictaions(id);
            var product = await  _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specifications);
            var productResult = _mapper.Map<ProductResultDto>(product);
            return productResult;
        }
    }
}
