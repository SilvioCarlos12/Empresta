using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Empresta.Ioc.Validation
{
    public sealed class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        public ValidationMiddleware(RequestDelegate next) { _next = next; }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {

                var resultado = Results.ValidationProblem(detail: ex.Message, errors: ex.Errors.ToDictionary(k => k.PropertyName, v => new[] { v.ErrorMessage }), statusCode: 400);

                await resultado.ExecuteAsync(context);
            }


        }
    }

}
