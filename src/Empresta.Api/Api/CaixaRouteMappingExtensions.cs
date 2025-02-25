using Empresta.Aplicacao.Commands;
using Empresta.Ioc.Extensao;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Empresta.Api.Api;

public static class CaixaRouteMappingExtensions
{
    private const string Caixa = "caixa";
    private const string WithTags = "Caixa";

    public static WebApplication MapCaixa(this WebApplication app, string rotaRaiz) =>
        app.MapAbrirCaixa(rotaRaiz)
            .MapFecharCaixa(rotaRaiz);

    private static WebApplication MapAbrirCaixa(this WebApplication app, string rotaRaiz)
    {
        app.MapPost($"{rotaRaiz}/{{id}}/{Caixa}", async ([FromServices] IMediator mediator,
                AbrirCaixaCommand cmd, Guid id, CancellationToken cancellationToken) =>
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
            })
            .Produces(201, typeof(AbrirCaixaSucesso))
            .Produces(400, typeof(AbrirCaixaInvalido))
            .Produces(404, typeof(AbrirCaixaNaoEncontrado))
            .Produces(500, typeof(AbrirCaixaErro))
            .WithMetadata(new SwaggerOperationAttribute("Abrir caixa", "Abri um caixa novo"))
            .WithTags(WithTags);

        return app;
    }

    private static WebApplication MapFecharCaixa(this WebApplication app, string rotaRaiz)
    {
        app.MapPatch($"{rotaRaiz}/{{id}}/{Caixa}", async ([FromServices] IMediator mediator,
                Guid id, CancellationToken cancellationToken) =>
            {
                FecharCaixaCommand cmd = new FecharCaixaCommand(id);

                var response = await mediator.Send(cmd, cancellationToken);
                return response switch
                {
                    FecharCaixaSucesso sucesso => Results.Ok(),
                    FecharCaixaInvalido invalido => Results.BadRequest(invalido.ErroDto),
                    FecharCaixaNaoEncontrado naoEncontrado => Results.NotFound(),
                    FecharCaixaErro erro => erro.ErroDto.ToResultPromblem(),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            })
            .Produces(200, typeof(FecharCaixaSucesso))
            .Produces(400, typeof(FecharCaixaInvalido))
            .Produces(404, typeof(FecharCaixaNaoEncontrado))
            .Produces(500, typeof(FecharCaixaErro))
            .WithMetadata(new SwaggerOperationAttribute("Fechar um caixa", "Fecha um caixa que está aberto"))
            .WithTags(WithTags);

        return app;
    }
}