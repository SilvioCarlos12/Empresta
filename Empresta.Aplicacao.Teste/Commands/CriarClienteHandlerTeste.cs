using System.Linq.Expressions;
using Empresta.Aplicacao.Commands;
using Empresta.Aplicacao.Mapper;
using Empresta.Aplicacao.MensagemErros;
using Empresta.Aplicacao.Teste.Builds;
using Empresta.Dominio;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
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
    public async Task DeveCadastrarOClienteComSucesso()
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
    
    [Fact]
    public async Task DeveRetornaNaoEncontradoQuantoFuncionarioNaoExiste()
    {
        //Arrange
        var command = CriarCommand();
        
        _funcionarioRepositorio.GetById(Arg.Any<Guid>(), CancellationToken.None)
            .ReturnsNullForAnyArgs();
        
        // Act
        
        var response = await _criarClienteHandler.Handle(command, CancellationToken.None);

        // Assert

        Assert.IsType<CriarClienteNaoEncontrado>(response);
    }
    
    [Fact]
    public async Task DeveRetornaErroQuandoClienteJaExisteParaFuncionario()
    {
        //Arrange
        var command = CriarCommand();
        
        var funcionarioNome = "nomeFuncionarioTeste";

        _clienteRepositorio.GetByFilter(x => x.Telefone.Dd + x.Telefone.Numero == command.Telefone.ToVo().TelefoneCompleto(),
            CancellationToken.None).ReturnsForAnyArgs([Cliente.Criar("tESTE",command.Telefone.ToVo(), command.Endereco.ToVo())]);
        
        _funcionarioRepositorio.GetById(Arg.Any<Guid>(), CancellationToken.None)
            .ReturnsForAnyArgs(Funcionario.Criar(funcionarioNome, command.Telefone.ToVo(), command.Endereco.ToVo()));
        
        _funcionarioRepositorio.GetByFilter(Arg.Any<Expression<Func<Funcionario,bool>>>(), CancellationToken.None)
            .ReturnsForAnyArgs([Funcionario.Criar(funcionarioNome, command.Telefone.ToVo(), command.Endereco.ToVo())]);
        
        // Act
        
        var response = await _criarClienteHandler.Handle(command, CancellationToken.None);
        
        // Assert

        Assert.IsType<CriarClienteInvalido>(response);
        
        var erro = (CriarClienteInvalido)response;
        
        Assert.Equal(MensagensErro.ClienteJaCadastradoParaEsseFuncionario, erro.ErroDtos.Single().Mensagem);
    }
    
    [Fact]
    public async Task DeveRetornaSucessoQuandoClienteExisteMasNaoExisteFuncionario()
    {
        //Arrange
        var command = CriarCommand();
        
        var funcionarioNome = "nomeFuncionarioTeste";

        _clienteRepositorio.GetByFilter(x => x.Telefone.Dd + x.Telefone.Numero == command.Telefone.ToVo().TelefoneCompleto(),
            CancellationToken.None).ReturnsForAnyArgs([Cliente.Criar("tESTE",command.Telefone.ToVo(), command.Endereco.ToVo())]);
        
        _funcionarioRepositorio.GetById(Arg.Any<Guid>(), CancellationToken.None)
            .ReturnsForAnyArgs(Funcionario.Criar(funcionarioNome, command.Telefone.ToVo(), command.Endereco.ToVo()));
        
        _funcionarioRepositorio.GetByFilter(Arg.Any<Expression<Func<Funcionario,bool>>>(), CancellationToken.None)
            .ReturnsForAnyArgs([]);
        
        // Act
        
        var response = await _criarClienteHandler.Handle(command, CancellationToken.None);
        
        // Assert

        Assert.IsType<CriarClienteSucesso>(response);
    }


    private static CriarClienteCommand CriarCommand()
    {
        var enderecoDto = new EnderecoDtoBuild().Build();
        
        var telefoneDto = new TelefoneDtoBuild().Build();
        
        var nome = "teste";
        
        var command = new CriarClienteCommand(nome, telefoneDto, enderecoDto);
        
        command.Id = Guid.NewGuid();
        
        return command;
    }
}