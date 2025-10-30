using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.OrderModule;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize]
    public class OrdersController(IServiceManager _serviceManager)  : ApiController
    {
        //CreateOrder
        [HttpPost]
        public async Task<ActionResult<OrderResult>> CreateOrderAsync(OrderRequest orderRequest)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email) ;
            var order = await _serviceManager.OrderSevice.CreateOrderAsync(orderRequest , userEmail);
            return Ok(order);
        }

        //GetOderById
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderResult>> GetOrderByIdAsync(Guid id)
        {
            var order = await _serviceManager.OrderSevice.GetOrderByIdAsync(id);
            return Ok(order);
        }

        //GetAllOrdersByEmail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResult>>> GetAllOrdersByEmailAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _serviceManager.OrderSevice.GetOrdersByEmailAsync(userEmail);
            return Ok(orders);
        }
        //GetDeliveryMethods
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodResult>>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _serviceManager.OrderSevice.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }
    }
}
