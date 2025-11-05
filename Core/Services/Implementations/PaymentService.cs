using AutoMapper;
using Domain.Contracts;
using Domain.Entities.OrderModule;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstraction.Contracts;
using Shared.Dtos.BasketModule;
using Stripe;
using Product = Domain.Entities.ProductModule.Product;

namespace Services.Implementations
{
    internal class PaymentService(IConfiguration _configuration , IBasketRepository _basketRepository , IUnitOfWork _unitOfWork , IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto?> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            //0- Install Stripe.net NuGet Package

            //1- Set your Stripe Secret Key
            StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["SecretKey"];
            
            //2- Get basket [by basketId]
            var basket = await _basketRepository.GetBasketAsync(basketId) 
                ?? throw new BasketNotFoundException(basketId);

            //3- Validate basket items prices ==> [basket.item.price = product.price] ==> product from db
            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }

            //4- Validate shipping price ==> Get deliveryMethod [ DeliveryMethodId ] ==> ShippingPrice ==> DeliveryMethod.Price
            if (!basket.DeliveryMethodId.HasValue) throw new Exception("No Delivery Method Selected!");

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(basket.DeliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Price;

            //5- Calculate total amount [subtotal + shipping]  ==> amount in cents * 100 ==> Long
            var subtotal = basket.BasketItems.Sum(item => item.Price * item.Quantity);
            var totalAmount = (long)((subtotal + basket.ShippingPrice) * 100);

            //6- Create or Update PaymentIntentId
            var stripeService = new PaymentIntentService();
            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                //Create new PaymentIntent
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = totalAmount,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                var paymentIntent = await stripeService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                //i- Product in db changed price
                //ii- User changed quantity of items in basket
                //iii- User changed shipping method
                //v- Any other change that affects the total amount
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = totalAmount //New Amount
                };
                await stripeService.UpdateAsync(basket.PaymentIntentId, options);
            }

            //7- Save changes to basket [ PaymentIntentId , ClientSecret ]
            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            //8- Map and return the basketDto
            return _mapper.Map<BasketDto>(basket);

        }
    }
}
