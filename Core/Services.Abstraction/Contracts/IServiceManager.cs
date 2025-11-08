namespace Services.Abstraction.Contracts
{
    public interface IServiceManager
    {
        public IProductService ProductService { get; }
        public IBasketService BasketService { get; }
        public IAuthenticationService AuthenticatioService { get; }
        public IOrderSevice OrderSevice { get; }
        public IPaymentService PaymentService { get; }
    }
}
