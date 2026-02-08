using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_Commerce.API.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult CustomValidationErrorResponse(ActionContext context)
        {
            //Context ==> errors , key [field]
            //Context.ModelState ==> ModelStateDictionary <string, ModelStateEntry>
            //string ==> Name of the field
            //ModelStateEntry ==> Errors collection ==> ErrorMessages

            var errors = context.ModelState
                .Where(error => error.Value?.Errors.Any() == true)
                .Select(error => new ValidationError()
                {
                    Field = error.Key,
                    Errors = error.Value?.Errors.Select(e => e.ErrorMessage) ?? new List<string>()
                });
            var response = new ValidationErrorResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorMessage = "One or more validation errors occurred.",
                Errors = errors
            };

            return new BadRequestObjectResult(response);
        }
    }
}
