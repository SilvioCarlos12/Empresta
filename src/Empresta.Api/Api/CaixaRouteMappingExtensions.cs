using Empresta.Aplicacao.Commands;
using Empresta.Ioc.Extensao;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Empresta.Api.Api;

public static class CaixaRouteMappingExtensions
{
    private const string _caixa = "caixa";
    public static WebApplication MapCaixa(this WebApplication app, string rotaRaiz) =>
        app.MapAbrirCaixa(rotaRaiz);
    
    private static WebApplication MapAbrirCaixa(this WebApplication app, string rotaRaiz)  {
        app.MapPost($"{rotaRaiz}/{{id}}/{_caixa}", async ([FromServices] IMediator mediator,
                AbrirCaixaCommand cmd,Guid id ,CancellationToken cancellationToken) =>
            {
                cmd.FuncionarioId = id;
                var response = await mediator.Send(cmd, cancellationToken);
                return response switch
                {
                    AbrirCaixaSucesso sucesso => Results.Created("", ""),
                    AbrirCaixaInvalido invalido => Results.BadRequest(invalido.ErroDto),
                    AbrirCaixaNaoEncontrado naoEncontrado => Results.NotFound(),
                    AbrirCaixaErro erro => erro.ErroDto.ToResultPromblem(),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            }).Produces(201, typeof(AbrirCaixaSucesso))
            .Produces(400, typeof(AbrirCaixaInvalido))
            .Produces(404, typeof(AbrirCaixaNaoEncontrado))
            .Produces(500, typeof(AbrirCaixaErro))
            .WithMetadata(new SwaggerOperationAttribute("Abrir caixa", "Abri um caixa novo"))
            .WithTags("Caixa");

        return app;
    }
}