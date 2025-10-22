using Domain.Contracts;
using E_Commerce.API.Extensions;
using E_Commerce.API.Factories;
using E_Commerce.API.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;
using Presistence.Data.Contexts;
using Presistence.Repositories;
using Services;
using Services.Abstraction.Contracts;
using Services.Implementations;

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
