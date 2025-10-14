using E_Commerce.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("{id}")] // GET api/Product/5
        public ActionResult<Product> GetProduct(int id)
        {
            return new Product() { Id = id };
        }

        [HttpGet("GetAllProducts")]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1 },
                new Product { Id = 2 },
                new Product { Id = 3 }
            };
            return products;
        }

        [HttpPost("CreateProduct")]
        public ActionResult<Product> CreateProduct(Product product)
        {
           return new Product() { Id = 2025 };
        }

        [HttpPut]
        public ActionResult<Product> UpdateProduct(Product product)
        {
            return new Product() { Id = 2025 };
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteProduct(int id)
        {
            return id > 20;
        }
    }
}
