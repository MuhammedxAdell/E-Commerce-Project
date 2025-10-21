using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        //EndPoint ==> GetAllProducts
        [HttpGet] //BaseUrl/api/products
        public async Task<ActionResult<IEnumerable<ProductResultDto>>> GetAllProducts(int? typeId , int? brandId)
            => Ok(await _serviceManager.ProductService.GetAllProductsAsync(typeId , brandId));

        //EndPoint ==> GetAllBrands
        [HttpGet("Brands")] //BaseUrl/api/products/Brands
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrands()
            => Ok(await _serviceManager.ProductService.GetAllBrandsAsync());

        //EndPoint ==> GetAllTypes
        [HttpGet("Types")] //BaseUrl/api/products/Types
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypes()
            => Ok(await _serviceManager.ProductService.GetAllTypesAsync());

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
