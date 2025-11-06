using Services;
using Services.Abstraction.Contracts;
using Services.Implementations;
using Shared.Common;

namespace E_Commerce.API.Extensions
{
    public static class CoreServicesExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => { }, typeof(AssemblyReference).Assembly);
            services.AddScoped<IServiceManager, ServiceManagerWithFactoryDelegate>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<Func<IProductService>>(provider => () => provider.GetRequiredService<IProductService>());

            services.AddScoped<IOrderSevice, OrderService>();
            services.AddScoped<Func<IOrderSevice>>(provider => () => provider.GetRequiredService<IOrderSevice>());

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<Func<IAuthenticationService>>(provider => () => provider.GetRequiredService<IAuthenticationService>());

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<Func<IPaymentService>>(provider => () => provider.GetRequiredService<IPaymentService>());

            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<Func<IBasketService>>(provider => () => provider.GetRequiredService<IBasketService>());

            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<Func<ICacheService>>(provider => () => provider.GetRequiredService<ICacheService>());

            services.Configure<JwtOption>(configuration.GetSection("JwtOptions")); //IOptions
            return services;
        }
    }
}
