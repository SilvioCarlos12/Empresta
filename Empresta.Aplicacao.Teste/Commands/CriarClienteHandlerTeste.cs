using Empresta.Aplicacao.Commands;
using Empresta.Aplicacao.Mapper;
using Empresta.Aplicacao.Teste.Builds;
using Empresta.Dominio;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using NSubstitute;
using Xunit;

namespace Empresta.Aplicacao.Teste.Commands;

public sealed class CriarClienteHandlerTeste
{
    private readonly  IClienteRepositorio _clienteRepositorio;
    private readonly  IFuncionarioRepositorio _funcionarioRepositorio;
    private readonly CriarClienteHandler _criarClienteHandler;

    public CriarClienteHandlerTeste()
    {
        _clienteRepositorio = Substitute.For<IClienteRepositorio>();
        _funcionarioRepositorio = Substitute.For<IFuncionarioRepositorio>();
        _criarClienteHandler = new CriarClienteHandler(_funcionarioRepositorio,_clienteRepositorio);
    }

    [Fact]
    public async Task DeveCadastrarCliente()
    {
        //Arrange
        var command = CriarCommand();
        
        var funcionarioNome = "nomeFuncionarioTeste";

        _clienteRepositorio.GetByFilter(x => x.Telefone.Dd + x.Telefone.Numero == command.Telefone.ToVo().TelefoneCompleto(),
            CancellationToken.None).ReturnsForAnyArgs(new List<Cliente>());
        
        _funcionarioRepositorio.GetById(Arg.Any<Guid>(), CancellationToken.None)
            .ReturnsForAnyArgs(Funcionario.Criar(funcionarioNome, command.Telefone.ToVo(), command.Endereco.ToVo()));
        
        // Act
        
        var response = await _criarClienteHandler.Handle(command, CancellationToken.None);

        // Assert

        Assert.IsType<CriarClienteSucesso>(response);
    }

    private static CriarClienteCommand CriarCommand()
    {
        var enderecoDto = new EnderecoDtoBuild().Build();
        
        var telefoneDto = new TelefoneDtoBuild().Buid();
        
        var nome = "teste";
        
        var command = new CriarClienteCommand(nome, telefoneDto, enderecoDto);
        
        command.Id = Guid.NewGuid();
        
        return command;
    }
}