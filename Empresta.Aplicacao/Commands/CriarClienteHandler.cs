using Empresta.Aplicacao.Dto;
using Empresta.Aplicacao.Mapper;
using Empresta.Aplicacao.MensagensErro;
using Empresta.Dominio;
using Empresta.Dominio.Vo;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;

namespace Empresta.Aplicacao.Commands
{
    public class CriarClienteHandler : IRequestHandler<CriarClienteCommand, CriarClienteResponse>
    {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;

        public CriarClienteHandler(IFuncionarioRepositorio funcionarioRepositorio, IClienteRepositorio clienteRepositorio)
        {
            _funcionarioRepositorio = funcionarioRepositorio;
            _clienteRepositorio = clienteRepositorio;
        }

        public async Task<CriarClienteResponse> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var funcionario = await _funcionarioRepositorio.GetById(request.Id, cancellationToken);

                if (funcionario is null)
                {
                    return CriarClienteResponse.NaoEncontrado();
                }
                

               await CadastrarCliente(request,funcionario,cancellationToken);


                return CriarClienteResponse.Sucesso();
            }
            catch (Exception ex)
            {

                return CriarClienteResponse.Error(new ErroDto(ex.Message, "000"));
            }
        }

        private async Task CadastrarCliente(CriarClienteCommand request,Funcionario funcionario ,CancellationToken cancellationToken)
        {
            Telefone telefone = request.Telefone.ToVo();

            var clienteExiste = await _clienteRepositorio
                                        .GetByFilter(x => x.Telefone.Dd+x.Telefone.Numero == telefone.TelefoneCompleto(), cancellationToken);

            if (clienteExiste.Any())
            {
                await AdicionarClienteExistenteNoFuncionario(funcionario, clienteExiste, cancellationToken);

                return;
            }

            var cliente = Cliente.Criar(request.Nome, telefone, request.Endereco.ToVo());

            await _clienteRepositorio.Add(cliente, cancellationToken);

            funcionario.AdicionarCliente(cliente);
            
            await _funcionarioRepositorio.Update(funcionario, cancellationToken);
        }

        private async Task AdicionarClienteExistenteNoFuncionario(Funcionario funcionario, IEnumerable<Cliente> clienteExiste, CancellationToken cancellationToken)
        {
            var clienteExisteNoFuncionario = await _funcionarioRepositorio.GetByFilter(x => x.Clientes
                                                                                              .Where(x => clienteExiste
                                                                                                            .Single()
                                                                                                            .Id.Equals(x.Id))
                                                                                              .Any(), cancellationToken);

            if (clienteExisteNoFuncionario.Count() == 1)
            {
                return;
            }

            funcionario.AdicionarCliente(clienteExiste.Single());

            await _funcionarioRepositorio.Update(funcionario, cancellationToken);
        }
    }

    public record CriarClienteCommand(string Nome, TelefoneDto Telefone, EnderecoDto Endereco) : IRequest<CriarClienteResponse>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }

    public record CriarClienteSucesso() : CriarClienteResponse;
    public record CriarClienteInvalido(params ErroDto[] ErroDtos) : CriarClienteResponse;
    public record CriarClienteErro(params ErroDto[] ErroDtos) : CriarClienteResponse;
    public record CriarClienteNaoEncontrado() : CriarClienteResponse;
    public record CriarClienteResponse
    {
        public static CriarClienteResponse Sucesso() => new CriarClienteSucesso();
        public static CriarClienteResponse Invalido(params ErroDto[] erroDtos) => new CriarClienteInvalido(erroDtos);
        public static CriarClienteResponse Error(params ErroDto[] erroDtos) => new CriarClienteErro(erroDtos);
        public static CriarClienteResponse NaoEncontrado() => new CriarClienteNaoEncontrado();
    }

    public class CriarClienteValidation : AbstractValidator<CriarClienteCommand>
    {
        public CriarClienteValidation()
        {
            RuleFor(x => x.Nome).NotEmpty();

            RuleFor(x => x.Telefone.NumeroTelefone)
                .Must(Telefone.ValidarNumero)
                .WithErrorCode(CodigosErros.NumeroTelefoneInvalido);

            RuleFor(x => x.Telefone.Dd)
             .Must(Telefone.ValidarDd)
             .WithErrorCode(CodigosErros.DdInvalido);

        }
    }
}
