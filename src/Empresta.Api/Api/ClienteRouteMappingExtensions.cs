using Empresta.Aplicacao.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Empresta.Api.Api
{
    public static class ClienteRouteMappingExtensions
    {
        private const string _cliente = "cliente";

        public static WebApplication MapCliente(this WebApplication app, string rotaRaiz) =>
            app.MapCriarCliente(rotaRaiz);

        private static WebApplication MapCriarCliente(this WebApplication app, string rotaRaiz)
        {
            app.MapPost($"{rotaRaiz}/{{id}}/{_cliente}", async ([FromServices] IMediator mediator,Guid id, 
                CriarClienteCommand criarClienteCommand,CancellationToken cancellationToken) =>
            {
                criarClienteCommand.Id = id;
                var response = await mediator.Send(criarClienteCommand, cancellationToken);
                return response switch
                {
                    CriarClienteSucesso sucesso => Results.Created("/cliente/{id}", ""),
                    CriarClienteInvalido invalido => Results.BadRequest(invalido.ErroDtos),
                    CriarClienteNaoEncontrado naoEncontrado => Results.NotFound(),
                    CriarClienteErro erro => Results.Problem(erro.ErroDtos.ToString()),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            }).Produces(201, typeof(CriarClienteSucesso))
                .Produces(400, typeof(CriarClienteInvalido))
                .Produces(404,typeof(CriarClienteNaoEncontrado))
                .Produces(500, typeof(CriarClienteErro))
                .WithMetadata(new SwaggerOperationAttribute("Criar um cliente", "Adicionar um novo cliente"))
                .WithTags("Cliente"); ;

            return app;
        }
    }
}
