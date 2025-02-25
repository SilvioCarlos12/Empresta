using Empresta.Aplicacao.Commands;
using Empresta.Aplicacao.Queries;
using Empresta.Ioc.Extensao;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Empresta.Api.Api
{
    public static class ClienteRouteMappingExtensions
    {
        private const string WithTags = "Cliente";
        private const string Cliente = "cliente";
        public static WebApplication MapCliente(this WebApplication app, string rotaRaiz) =>
            app.MapCriarCliente(rotaRaiz)
               .MapObterCliente(rotaRaiz)
               .MapAtualizarCliente(rotaRaiz);

        private static WebApplication MapCriarCliente(this WebApplication app, string rotaRaiz)
        {
            app.MapPost($"{rotaRaiz}/{{id}}/{Cliente}", async ([FromServices] IMediator mediator, Guid id,
                CriarClienteCommand criarClienteCommand, CancellationToken cancellationToken) =>
            {
                criarClienteCommand.Id = id;
                var response = await mediator.Send(criarClienteCommand, cancellationToken);
                return response switch
                {
                    CriarClienteSucesso sucesso => Results.Created($"/{Cliente}/{{id}}", ""),
                    CriarClienteInvalido invalido => Results.BadRequest(invalido.ErroDtos),
                    CriarClienteNaoEncontrado naoEncontrado => Results.NotFound(),
                    CriarClienteErro erro => erro.ErroDto.ToResultPromblem(),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            }).Produces(201, typeof(CriarClienteSucesso))
                .Produces(400, typeof(CriarClienteInvalido))
                .Produces(404, typeof(CriarClienteNaoEncontrado))
                .Produces(500, typeof(CriarClienteErro))
                .WithMetadata(new SwaggerOperationAttribute("Criar um cliente", "Adicionar um novo cliente"))
                .WithTags(WithTags);

            return app;
        }


        private static WebApplication MapObterCliente(this WebApplication app, string rotaRaiz)
        {
            app.MapGet($"{rotaRaiz}/{{id}}/{Cliente}", async ([FromServices] IMediator mediator, Guid id, CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(new BuscarClientesPorFuncionarioIdQuery(id), cancellationToken);
                return response switch
                {
                    BuscarClientesPorFuncionarioIdSucesso sucesso => Results.Ok(sucesso),
                    BuscarClientesPorFuncionarioIdNaoEncontrado naoEncontrado => Results.NotFound(),
                    BuscarClientesPorFuncionarioIdErro erro => erro.ErroDto.ToResultPromblem(),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            }).Produces(200, typeof(BuscarClientesPorFuncionarioIdSucesso))
                .Produces(404, typeof(BuscarClientesPorFuncionarioIdNaoEncontrado))
                .Produces(500, typeof(BuscarClientesPorFuncionarioIdErro))
                .WithMetadata(new SwaggerOperationAttribute("Obtem clientes de um funcionario", "Obter clientes de um funcionario"))
                .WithTags(WithTags);

            return app;
        }

        private static WebApplication MapAtualizarCliente(this WebApplication app, string rotaRaiz)
        {
            app.MapPut($"{rotaRaiz}/{{funcioarioId}}/{Cliente}/{{clienteId}}", async ([FromServices] IMediator mediator, Guid funcioarioId,Guid clienteId,
                AtualizarClienteDeFuncionarioCommand cmd
                , CancellationToken cancellationToken) =>
            {
                cmd.FuncionarioId = funcioarioId;
                cmd.ClienteId = clienteId;
                var response = await mediator.Send(cmd, cancellationToken);
                return response switch
                {
                    AtualizarClienteDeFuncionarioSucesso sucesso => Results.Ok(sucesso),
                    AtualizarClienteDeFuncionarioNaoEncontrado naoEncontrado => Results.NotFound(),
                    AtualizarClienteDeFuncionarioErro erro => erro.ErroDto.ToResultPromblem(),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            }).Produces(200, typeof(AtualizarClienteDeFuncionarioSucesso))
                .Produces(404, typeof(AtualizarClienteDeFuncionarioNaoEncontrado))
                .Produces(500, typeof(AtualizarClienteDeFuncionarioErro))
                .WithMetadata(new SwaggerOperationAttribute("Atualizar um cliente de um funcionario", "Atualizar um cliente de um funcionario"))
                .WithTags(WithTags);

            return app;
        }

    }
}
