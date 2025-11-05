using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstraction.Contracts;
using Services.Specifications;
using Shared.Dtos.BasketModule;
using Stripe;
using Product = Domain.Entities.ProductModule.Product;
using Order = Domain.Entities.OrderModule.Order;

namespace Services.Implementations
{
    public class PaymentService(IConfiguration _configuration, IBasketRepository _basketRepository, IUnitOfWork _unitOfWork, IMapper _mapper) : IPaymentService
    {
        //public async Task<BasketDto?> CreateOrUpdatePaymentIntentAsync(string basketId)
        //{
        //    //0- Install Stripe.net NuGet Package

        //    //1- Set your Stripe Secret Key
        //    StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["SecretKey"];

        //    //2- Get basket [by basketId]
        //    var basket = await _basketRepository.GetBasketAsync(basketId) 
        //        ?? throw new BasketNotFoundException(basketId);

        //    //3- Validate basket items prices ==> [basket.item.price = product.price] ==> product from db
        //    foreach (var item in basket.BasketItems)
        //    {
        //        var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id)
        //            ?? throw new ProductNotFoundException(item.Id);
        //        item.Price = product.Price;
        //    }

        //    //4- Validate shipping price ==> Get deliveryMethod [ DeliveryMethodId ] ==> ShippingPrice ==> DeliveryMethod.Price
        //    if (!basket.DeliveryMethodId.HasValue) throw new Exception("No Delivery Method Selected!");

        //    var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
        //        .GetByIdAsync(basket.DeliveryMethodId.Value)
        //        ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
        //    basket.ShippingPrice = deliveryMethod.Price;

        //    //5- Calculate total amount [subtotal + shipping]  ==> amount in cents * 100 ==> Long
        //    var subtotal = basket.BasketItems.Sum(item => item.Price * item.Quantity);
        //    var totalAmount = (long)((subtotal + basket.ShippingPrice) * 100);

        //    //6- Create or Update PaymentIntentId
        //    var stripeService = new PaymentIntentService();
        //    if(string.IsNullOrEmpty(basket.PaymentIntentId))
        //    {
        //        //Create new PaymentIntent
        //        var options = new PaymentIntentCreateOptions()
        //        {
        //            Amount = totalAmount,
        //            Currency = "usd",
        //            PaymentMethodTypes = new List<string> { "card" }
        //        };
        //        var paymentIntent = await stripeService.CreateAsync(options);
        //        basket.PaymentIntentId = paymentIntent.Id;
        //        basket.ClientSecret = paymentIntent.ClientSecret;
        //    }
        //    else
        //    {
        //        //i- Product in db changed price
        //        //ii- User changed quantity of items in basket
        //        //iii- User changed shipping method
        //        //v- Any other change that affects the total amount
        //        var options = new PaymentIntentUpdateOptions()
        //        {
        //            Amount = totalAmount //New Amount
        //        };
        //        await stripeService.UpdateAsync(basket.PaymentIntentId, options);
        //    }

        //    //7- Save changes to basket [ PaymentIntentId , ClientSecret ]
        //    await _basketRepository.CreateOrUpdateBasketAsync(basket);

        //    //8- Map and return the basketDto
        //    return _mapper.Map<BasketDto>(basket);

        //}

        public async Task<BasketDto?> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["SecretKey"];
            
            var basket = await GetBasketAsync(basketId);

            await ValidateBasketItemsPricesAsync(basket);

            await ValidateAndSetShippingPriceAsync(basket);

            var amount = CalculateTotalAmountAsync(basket);

            await CreateOrUpdatePaymentIntentInStripeAsync(basket, amount);

            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            return _mapper.Map<BasketDto>(basket);

        }

        private async Task CreateOrUpdatePaymentIntentInStripeAsync(CustomerBasket basket, long amount)
        {
            var stripeService = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                var paymentIntent = await stripeService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = amount
                };
                await stripeService.UpdateAsync(basket.PaymentIntentId, options);
            }
        }

        private long CalculateTotalAmountAsync(CustomerBasket basket)
        {
            var amount = (long)((basket.Items.Sum(item => item.Price * item.Quantity) + basket.ShippingPrice) * 100);
            return amount;
        }

        private async Task ValidateAndSetShippingPriceAsync(CustomerBasket basket)
        {
            if (!basket.DeliveryMethodId.HasValue) throw new Exception("No Delivery Method Selected!");
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(basket.DeliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Price;
        }

        private async Task ValidateBasketItemsPricesAsync(CustomerBasket basket)
        {
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }
        }

        private async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            return await _basketRepository.GetBasketAsync(basketId)
                ?? throw new BasketNotFoundException(basketId);
        }

        public async Task UpadatePaymentStatusAsync(string json, string signatureHeader)
        {
            string endpointSecret = _configuration.GetSection("StripeSettings")["EndpointSecret"];
   
            var stripeEvent = EventUtility.ParseEvent(json ,throwOnApiVersionMismatch:false );
            
            stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret , throwOnApiVersionMismatch: false);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                //Change order status to PaymentReceived
                await UpdatePaymentStatusRevievedAsync(paymentIntent.Id);
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                //Change order status to PaymentFailed
                await UpdatePaymentStatusFailedAsync(paymentIntent.Id);
            }
            else
            {
                // Unexpected event type
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }            
        }

        private async Task UpdatePaymentStatusFailedAsync(string paymentIntentId)
        {
            var oderRepository = _unitOfWork.GetRepository<Order, Guid>();
            var order = await oderRepository
                .GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));

            if (order is not null)
            {
                order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
                oderRepository.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        private async Task UpdatePaymentStatusRevievedAsync(string paymentIntentId)
        {
            var oderRepository = _unitOfWork.GetRepository<Order, Guid>();
            var order = await oderRepository
                .GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));

            if (order is not null)
            {
                order.PaymentStatus = OrderPaymentStatus.PaymentRecived;
                oderRepository.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}