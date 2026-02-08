using Domain.Contracts;
using E_Commerce.API.Middlewares;

namespace E_Commerce.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dataSeedingObject = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await dataSeedingObject.SeedDataAsync();
            await dataSeedingObject.SeedIdentityDataAsync();

            return app;
        }

        public static WebApplication UseExceptionHandlingMiddleWare(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            return app;
        }

        public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
        {
            app.UseSwagger();   //Middlewares ==> swagger
            app.UseSwaggerUI(); //Middlewares ==> swagger
            return app;
        }
    }
}
