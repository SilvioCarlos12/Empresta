using Empresta.Aplicacao.Dto;
using Empresta.Aplicacao.Mapper;
using Empresta.Aplicacao.MensagemErros;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using FluentValidation;
using MediatR;

namespace Empresta.Aplicacao.Queries
{
    public class BuscarClientesPorFuncionarioIdHandler : IRequestHandler<BuscarClientesPorFuncionarioIdQuery, BuscarClientesPorFuncionarioIdResponse>
    {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;

        public BuscarClientesPorFuncionarioIdHandler(IFuncionarioRepositorio funcionarioRepositorio)
        {
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        public async Task<BuscarClientesPorFuncionarioIdResponse> Handle(BuscarClientesPorFuncionarioIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var funcionario = await _funcionarioRepositorio.GetById(request.Id, cancellationToken);

                if (funcionario is null)
                {
                    return BuscarClientesPorFuncionarioIdResponse.NaoEncontrado();
                }

                return BuscarClientesPorFuncionarioIdResponse.Sucesso(funcionario
                                                                      .Clientes
                                                                      .Select(x => x.ToDto()));
            }
            catch (Exception ex)
            {

                return BuscarClientesPorFuncionarioIdResponse
                    .Erro(new ErroDto(CodigosErros.ErroSistematico, ex.Message));
            }
        }
    }

    public record BuscarClientesPorFuncionarioIdQuery(Guid Id) : IRequest<BuscarClientesPorFuncionarioIdResponse>;
    public record BuscarClientesPorFuncionarioIdSucesso(IEnumerable<ClienteDto> Clientes) : BuscarClientesPorFuncionarioIdResponse;
    public record BuscarClientesPorFuncionarioIdNaoEncontrado() : BuscarClientesPorFuncionarioIdResponse;
    public record BuscarClientesPorFuncionarioIdErro(ErroDto ErroDtos) : BuscarClientesPorFuncionarioIdResponse;
    public record BuscarClientesPorFuncionarioIdResponse
    {
        public static BuscarClientesPorFuncionarioIdResponse Sucesso(IEnumerable<ClienteDto> clientes) => new BuscarClientesPorFuncionarioIdSucesso(clientes);
        public static BuscarClientesPorFuncionarioIdResponse NaoEncontrado() => new BuscarClientesPorFuncionarioIdNaoEncontrado();
        public static BuscarClientesPorFuncionarioIdResponse Erro(ErroDto erroDtos) => new BuscarClientesPorFuncionarioIdErro(erroDtos);
    }

    public class BuscarClientesPorFuncionarioIdValidacao : AbstractValidator<BuscarClientesPorFuncionarioIdQuery>
    {

    }
}
