using Services.Abstraction.Contracts;

namespace Services.Implementations
{
    public class ServiceManagerWithFactoryDelegate(Func<IProductService> _productFactory , Func<IOrderSevice> _orderFactory,
        Func<IAuthenticationService> _authFactory , Func<IPaymentService> _paymentFactory , Func<IBasketService> _basketFactory ,
        Func<ICacheService> _cacheFactory) : IServiceManager
    {
        public IProductService ProductService => _productFactory.Invoke();

        public IBasketService BasketService => _basketFactory.Invoke();

        public IAuthenticationService AuthenticatioService => _authFactory.Invoke();

        public IOrderSevice OrderSevice => _orderFactory.Invoke();

        public IPaymentService PaymentService => _paymentFactory.Invoke();
        public ICacheService CacheService => _cacheFactory.Invoke();
    }
}
