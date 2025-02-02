using Empresta.Aplicacao.Dto;
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

                var resultado = Results.BadRequest(ex.Errors.Select(x => new ErroDto(x.ErrorCode, x.ErrorMessage)));

                await resultado.ExecuteAsync(context);
            }


        }
    }

}
