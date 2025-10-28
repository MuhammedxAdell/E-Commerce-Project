using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;
using Presistence.Data.Contexts;
using Presistence.Identity;
using Presistence.Repositories;
using StackExchange.Redis;

namespace E_Commerce.API.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<IdentityStoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });
            services.AddScoped<IDataSeeding, DataSeeding>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
               return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
            });
            services.AddScoped<IBasketRepository, BasketRepository>();

            return services;
        }
    }
}
