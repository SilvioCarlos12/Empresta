namespace Empresta.Api.Api
{
    public static class RouteMappingExtensions
    {
        private const string Funcionario = "funcionario";

        public static WebApplication MapRotas(this WebApplication app) => app
            .MapCliente(Funcionario)
            .MapCaixa(Funcionario);
    }
}