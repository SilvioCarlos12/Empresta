using Empresta.Aplicacao.Dto;
using Empresta.Aplicacao.Mapper;
using Empresta.Aplicacao.MensagemErros;
using Empresta.Aplicacao.Validacao;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;

namespace Empresta.Aplicacao.Commands
{
    public sealed class AtualizarClienteDeFuncionarioHandler : IRequestHandler<AtualizarClienteDeFuncionarioCommand, AtualizarClienteDeFuncionarioResponse>
    {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;

        public AtualizarClienteDeFuncionarioHandler(IFuncionarioRepositorio funcionarioRepositorio, IClienteRepositorio clienteRepositorio)
        {
            _funcionarioRepositorio = funcionarioRepositorio;
            _clienteRepositorio = clienteRepositorio;
        }

        public async Task<AtualizarClienteDeFuncionarioResponse> Handle(AtualizarClienteDeFuncionarioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var funcionario = await _funcionarioRepositorio.GetById(request.FuncionarioId, cancellationToken);

                if (funcionario == null)
                {
                    return AtualizarClienteDeFuncionarioResponse.NaoEncontrado();
                }

                var cliente = funcionario.Clientes.SingleOrDefault(x => x.Id == request.ClienteId);

                if (cliente == null)
                {
                    return AtualizarClienteDeFuncionarioResponse.NaoEncontrado();
                }

                cliente.AtualizarCliente(request.Nome, request.Telefone.ToVo(), request.Endereco.ToVo());

                funcionario.AtualizarClienteDoFuncionario(cliente);
                

                await _funcionarioRepositorio.Update(funcionario, cancellationToken);

                await _clienteRepositorio.Update(cliente, cancellationToken);

                return AtualizarClienteDeFuncionarioResponse.Sucesso();

            }
            catch (Exception ex)
            {
                return AtualizarClienteDeFuncionarioResponse.Erro(new ErroDto(CodigosErros.ErroSistematico, ex.Message));
            }
        }
    }

    public sealed record AtualizarClienteDeFuncionarioCommand(string Nome, TelefoneDto Telefone, EnderecoDto Endereco) : IRequest<AtualizarClienteDeFuncionarioResponse>
    {
        [JsonIgnore]
        public Guid FuncionarioId { get; set; }

        [JsonIgnore]
        public Guid ClienteId { get; set; }

    }

    public sealed record AtualizarClienteDeFuncionarioSucesso() : AtualizarClienteDeFuncionarioResponse;
    public sealed record AtualizarClienteDeFuncionarioNaoEncontrado() : AtualizarClienteDeFuncionarioResponse;
    public sealed record AtualizarClienteDeFuncionarioErro(ErroDto ErroDto) : AtualizarClienteDeFuncionarioResponse;

    public record AtualizarClienteDeFuncionarioResponse
    {
        public static AtualizarClienteDeFuncionarioResponse Sucesso() => new AtualizarClienteDeFuncionarioSucesso();
        public static AtualizarClienteDeFuncionarioResponse NaoEncontrado() => new AtualizarClienteDeFuncionarioNaoEncontrado();
        public static AtualizarClienteDeFuncionarioResponse Erro(ErroDto erro) => new AtualizarClienteDeFuncionarioErro(erro);

    }

    public sealed class AtualizarClienteDeFuncionarioValidation : AbstractValidator<AtualizarClienteDeFuncionarioCommand>
    {
        public AtualizarClienteDeFuncionarioValidation()
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
}
