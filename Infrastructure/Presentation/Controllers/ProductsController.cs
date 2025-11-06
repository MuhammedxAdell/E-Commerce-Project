using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using Services.Abstraction.Contracts;
using Shared;
using Shared.Dtos.ProductModule;
using Shared.Enums;
using Shared.ErrorModels;

namespace Presentation.Controllers
{
    public class ProductsController(IServiceManager _serviceManager) : ApiController
    {
        //EndPoint ==> GetAllProducts
        [RedisCache]
        [HttpGet] //BaseUrl/api/products
        public async Task<ActionResult<PaginatedResult<ProductResultDto>>> GetAllProducts([FromQuery]ProductSpecificationParameters parameters)
            => Ok(await _serviceManager.ProductService.GetAllProductsAsync(parameters));

        //EndPoint ==> GetAllBrands
        [HttpGet("Brands")] //BaseUrl/api/products/Brands
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrands()
            => Ok(await _serviceManager.ProductService.GetAllBrandsAsync());

        //EndPoint ==> GetAllTypes
        [HttpGet("Types")] //BaseUrl/api/products/Types
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypes()
            => Ok(await _serviceManager.ProductService.GetAllTypesAsync());

        [ProducesResponseType(typeof(ProductResultDto), StatusCodes.Status200OK)]
        //EndPoint ==> GetProductById
        [HttpGet("{id:int}")] //BaseUrl/api/products/3
        public async Task<ActionResult<ProductResultDto>> GetProductById(int id)
        {
            var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
            if (product is null)
                return NotFound();
            return Ok(product);
        }
    }
}
