using E_Commerce.API.Extensions;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region DI Container

            // Web API Services
            builder.Services.AddWebApiServiers();

            //Infrastructure Services
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // Core Services
            builder.Services.AddCoreServices();

            #endregion

            #region Pipelines - Middlewares

            var app = builder.Build();
            await app.SeedDataAsync();


            //Middleware ==> Handle exceptions
            app.UseExceptionHandlingMiddleWare();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            app.Run(); 

            #endregion
        }
    }
}
