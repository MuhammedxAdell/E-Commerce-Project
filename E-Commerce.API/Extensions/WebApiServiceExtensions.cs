using E_Commerce.API.Factories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Extensions
{
    public static class WebApiServiceExtensions
    {
        public static IServiceCollection AddWebApiServiers(this IServiceCollection services , IConfiguration _configuration)
        {
            services.AddControllers();
            var frontendUrl = _configuration.GetSection("URLS")["ClientUrl"];
            services.AddCors(options =>
            {
                //URL of the Angular app
                //Headers , Methods [GET, POST ,..]
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .WithOrigins(frontendUrl);
                });
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            });
            return services;
        }
    }
}
