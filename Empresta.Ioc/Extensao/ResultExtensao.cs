using Empresta.Aplicacao.Dto;
using Microsoft.AspNetCore.Http;

namespace Empresta.Ioc.Extensao
{
    public static class ResultExtensao
    {
        public static IResult ToResultPromblem(this  ErroDto result)
        {
            return Results.Problem(statusCode: int.Parse(result.CodigoErro), detail: result.Mensagem);
        }
    }
}
