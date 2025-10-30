using AutoMapper;
using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Abstraction.Contracts;
using Shared.Common;

namespace Services.Implementations
{
    public class ServiceManager(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basketRepository, UserManager<User> _userManager, IOptions<JwtOption> _options) : IServiceManager
    {
        private readonly Lazy<IProductService> _productService = new Lazy<IProductService>(() => new ProductService(_unitOfWork , _mapper));
        private readonly Lazy<IBasketService> _basketService = new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));
        private readonly Lazy<IAuthenticationService> _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(_userManager , _options));
        private readonly Lazy<IOrderSevice> _orderService = new Lazy<IOrderSevice>(() => new OrderService( _mapper ,  _basketRepository ,  _unitOfWork));
        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _basketService.Value;

        public IAuthenticationService AuthenticatioService => _authenticationService.Value;

        public IOrderSevice OrderSevice => _orderService.Value;
    }
}
