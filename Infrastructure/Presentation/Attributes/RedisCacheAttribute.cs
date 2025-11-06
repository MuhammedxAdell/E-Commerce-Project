using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstraction.Contracts;
using System.Text;

namespace Presentation.Attributes
{
    internal class RedisCacheAttribute(int durationInSeconds = 120) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;
            //Data cached or not ==> key
            //Key ==> PathUrl + QueryString

            string cacheKey = GenerateKey(context.HttpContext.Request);
            var result = await cacheService.GetCachedValueAsync(cacheKey);
            if ( result is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = result,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            var resultContext = await next.Invoke(); // execute the action method
            if ( resultContext.Result is OkObjectResult okObjectResult)
            {
                //Store data in redis cache
                await cacheService.SetCacheValueAsync(cacheKey, okObjectResult, TimeSpan.FromSeconds(durationInSeconds));
            }

        }

        private string GenerateKey(HttpRequest request)
        {
            //string variable ==> add path/api/products
            //variable ==> add query string values
            var key = new StringBuilder();
            key.Append(request.Path); // /api/products
            foreach( var item in request.Query.OrderBy( x => x.Key))
            {
                key.Append($"{item.Key}-{item.Value}-"); // categoryId-5-sort-price
            }
            return key.ToString();
        }
    }
}
