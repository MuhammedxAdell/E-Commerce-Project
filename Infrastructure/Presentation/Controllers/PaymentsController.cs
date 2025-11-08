using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.BasketModule;

namespace Presentation.Controllers
{
    public class PaymentsController(IServiceManager _serviceManager ) : ApiController
    {
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            return Ok(await _serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId));
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signatureHeader = Request.Headers["Stripe-Signature"];

            await _serviceManager.PaymentService.UpadatePaymentStatusAsync(json, signatureHeader);
            return new EmptyResult();
        }
    }
}
