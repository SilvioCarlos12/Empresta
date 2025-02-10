using Empresta.Aplicacao.Dto;
using Empresta.Aplicacao.Mapper;
using Empresta.Aplicacao.MensagemErros;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using FluentValidation;
using MediatR;

namespace Empresta.Aplicacao.Queries;

public sealed class BuscarClientesPorFuncionarioIdHandler : IRequestHandler<BuscarClientesPorFuncionarioIdQuery, BuscarClientesPorFuncionarioIdResponse>
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

public sealed record BuscarClientesPorFuncionarioIdQuery(Guid Id) : IRequest<BuscarClientesPorFuncionarioIdResponse>;
public sealed record BuscarClientesPorFuncionarioIdSucesso(IEnumerable<ClienteDto> Clientes) : BuscarClientesPorFuncionarioIdResponse;
public sealed record BuscarClientesPorFuncionarioIdNaoEncontrado() : BuscarClientesPorFuncionarioIdResponse;
public sealed record BuscarClientesPorFuncionarioIdErro(ErroDto ErroDto) : BuscarClientesPorFuncionarioIdResponse;
public record BuscarClientesPorFuncionarioIdResponse
{
    public static BuscarClientesPorFuncionarioIdResponse Sucesso(IEnumerable<ClienteDto> clientes) => new BuscarClientesPorFuncionarioIdSucesso(clientes);
    public static BuscarClientesPorFuncionarioIdResponse NaoEncontrado() => new BuscarClientesPorFuncionarioIdNaoEncontrado();
    public static BuscarClientesPorFuncionarioIdResponse Erro(ErroDto erroDto) => new BuscarClientesPorFuncionarioIdErro(erroDto);
}

public sealed class BuscarClientesPorFuncionarioIdValidacao : AbstractValidator<BuscarClientesPorFuncionarioIdQuery>
{
    public BuscarClientesPorFuncionarioIdValidacao()
    {
            
    }
}