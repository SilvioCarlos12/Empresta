using Empresta.Aplicacao.Dto;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Results;

namespace Empresta.Ioc.Validation
{
    public class ValidadorResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IResult CreateResult(EndpointFilterInvocationContext context, FluentValidation.Results.ValidationResult validationResult)
        {
            var validationProblemErrors = validationResult.Errors.Select(x=>new ErroDto(x.ErrorCode,x.ErrorMessage));
            return Results.BadRequest(validationProblemErrors);
        }
    }
}
