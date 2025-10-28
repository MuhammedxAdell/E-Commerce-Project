using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.BasketModule;

namespace Presentation.Controllers
{
    public class BasketController(IServiceManager _serviceManager) : ApiController
    {
        //Get
        [HttpGet]
        public async Task<ActionResult> GetBasketAsync(string basketId)
        => Ok(await _serviceManager.BasketService.GetBasketAsync(basketId));
        //Post 
        [HttpPost]
        public async Task<ActionResult> CreateOrUpdateBasketAsync(BasketDto basketDto)
        => Ok(await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basketDto));
        //Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBasketAsync(string id)
        {
            await _serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }

    }
}
