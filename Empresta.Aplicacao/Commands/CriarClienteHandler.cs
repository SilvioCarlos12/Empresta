using Empresta.Aplicacao.Dto;
using Empresta.Aplicacao.Mapper;
using Empresta.Aplicacao.MensagemErros;
using Empresta.Aplicacao.Validacao;
using Empresta.Dominio;
using Empresta.Dominio.Vo;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;

namespace Empresta.Aplicacao.Commands;

public sealed class CriarClienteHandler(
    IFuncionarioRepositorio funcionarioRepositorio,
    IClienteRepositorio clienteRepositorio)
    : IRequestHandler<CriarClienteCommand, CriarClienteResponse>
{
    public async Task<CriarClienteResponse> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var funcionario = await funcionarioRepositorio.GetById(request.Id, cancellationToken);

            if (funcionario is null)
            {
                return CriarClienteResponse.NaoEncontrado();
            }

            return await CadastrarCliente(request, funcionario, cancellationToken);
        }
        catch (Exception ex)
        {
            return CriarClienteResponse.Error(new ErroDto(CodigosErros.ErroSistematico, ex.Message));
        }
    }

    private async Task<CriarClienteResponse> CadastrarCliente(CriarClienteCommand request, Funcionario funcionario, CancellationToken cancellationToken)
    {
        Telefone telefone = request.Telefone.ToVo();

        var clienteExiste = await clienteRepositorio
            .GetByFilter(x => x.Telefone.Dd + x.Telefone.Numero == telefone.TelefoneCompleto(), cancellationToken);

        if (clienteExiste.Count == 1)
        {

            return await AdicionarClienteExistenteNoFuncionario(funcionario, clienteExiste, cancellationToken);
        }

        var cliente = Cliente.Criar(request.Nome, telefone, request.Endereco.ToVo());

        await clienteRepositorio.Add(cliente, cancellationToken);

        funcionario.AdicionarCliente(cliente);

        await funcionarioRepositorio.Update(funcionario, cancellationToken);

        return CriarClienteResponse.Sucesso();
    }

    private async Task<CriarClienteResponse> AdicionarClienteExistenteNoFuncionario(Funcionario funcionario, IEnumerable<Cliente> clienteExiste, CancellationToken cancellationToken)
    {
        var clienteExisteNoFuncionario = await funcionarioRepositorio.GetByFilter(x => x.Clientes
            .Any(cliente => clienteExiste
                .Single()
                .Id.Equals(cliente.Id)), cancellationToken);

        if (clienteExisteNoFuncionario.Count == 1)
        {
            return CriarClienteResponse.Invalido(
                new ErroDto(CodigosErros.ClienteJaCadastradoParaEsseFuncionario,
                    MensagensErro.ClienteJaCadastradoParaEsseFuncionario));
        }

        funcionario.AdicionarCliente(clienteExiste.Single());

        await funcionarioRepositorio.Update(funcionario, cancellationToken);

        return CriarClienteResponse.Sucesso();
    }
}

public sealed record CriarClienteCommand(string Nome, TelefoneDto Telefone, EnderecoDto Endereco) : IRequest<CriarClienteResponse>
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public sealed record CriarClienteSucesso : CriarClienteResponse;
public sealed record CriarClienteInvalido(params ErroDto[] ErroDtos) : CriarClienteResponse;
public sealed record CriarClienteErro(ErroDto ErroDto) : CriarClienteResponse;
public sealed record CriarClienteNaoEncontrado : CriarClienteResponse;
public  record CriarClienteResponse
{
    public static CriarClienteResponse Sucesso() => new CriarClienteSucesso();
    public static CriarClienteResponse Invalido(params ErroDto[] erroDtos) => new CriarClienteInvalido(erroDtos);
    public static CriarClienteResponse Error(ErroDto erroDto) => new CriarClienteErro(erroDto);
    public static CriarClienteResponse NaoEncontrado() => new CriarClienteNaoEncontrado();
}

public sealed class CriarClienteValidation : AbstractValidator<CriarClienteCommand>
{
    public CriarClienteValidation()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithErrorCode(CodigosErros.NomeEObrigatorio)
            .WithMessage(MensagensErro.NomeEObrigatorio);

        RuleFor(x => x.Endereco)
            .SetValidator(EnderecoDtoValidar.ObterValidacao());

        RuleFor(x => x.Telefone)
            .SetValidator(TelefoneDtoValidar.ObterValidacao());
    }
}