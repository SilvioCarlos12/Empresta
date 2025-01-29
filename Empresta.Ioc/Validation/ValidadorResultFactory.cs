using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Results;
using SharpGrip.FluentValidation.AutoValidation.Shared.Extensions;

namespace Empresta.Ioc.Validation
{
    public class ValidadorResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IResult CreateResult(EndpointFilterInvocationContext context, FluentValidation.Results.ValidationResult validationResult)
        {
            var validationProblemErrors = validationResult.ToValidationProblemErrors();
            return Results.ValidationProblem(validationProblemErrors, "Erro de validação");
        }
    }
}
