using Empresta.Aplicacao.Mapper;
using Empresta.Aplicacao.Queries;
using Empresta.Aplicacao.Teste.Builds;
using Empresta.Dominio;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Empresta.Aplicacao.Teste.Queries
{
    public sealed class BuscarClientePorFuncionarioIdHandlerTeste
    {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        private BuscarClientesPorFuncionarioIdHandler _buscarClientesPorFuncionarioIdHandler;

        public BuscarClientePorFuncionarioIdHandlerTeste()
        {
            _funcionarioRepositorio = Substitute.For<IFuncionarioRepositorio>();
            _buscarClientesPorFuncionarioIdHandler = new BuscarClientesPorFuncionarioIdHandler(_funcionarioRepositorio);


        }

        [Fact]
        public async Task DevoRetornaNaoEncontradoQuandoFuncionarioNaoExiste()
        {
            //Arrange

            _funcionarioRepositorio.GetById(Arg.Any<Guid>(), CancellationToken.None).ReturnsNull();

            var query = new BuscarClientesPorFuncionarioIdQuery(Guid.NewGuid());

            //Act

            var resultado = await _buscarClientesPorFuncionarioIdHandler.Handle(query, CancellationToken.None);

            // Asserts

            Assert.IsType<BuscarClientesPorFuncionarioIdNaoEncontrado>(resultado);
        }

        [Fact]
        public async Task DevoRetornaClientesDeFuncionarioExistente()
        {
            //Arrange
            var telefone = new TelefoneDtoBuild().Build();

            var endereco =  new EnderecoDtoBuild().Build();

            var cliente = new ClienteDtoBuild().Build();

            var funcionario = Funcionario.Criar("teste", telefone.ToVo(), endereco.ToVo());

            funcionario.AdicionarCliente(cliente.ToEntidade());

            _funcionarioRepositorio.GetById(Arg.Any<Guid>(), CancellationToken.None).Returns(funcionario);

            var query = new BuscarClientesPorFuncionarioIdQuery(Guid.NewGuid());

            //Act

            var resultado = await _buscarClientesPorFuncionarioIdHandler.Handle(query, CancellationToken.None);

            // Asserts

            Assert.IsType<BuscarClientesPorFuncionarioIdSucesso>(resultado);

            var respostasComClientes = (BuscarClientesPorFuncionarioIdSucesso)resultado;

            Assert.Contains(funcionario.Clientes.First().ToDto(), respostasComClientes.Clientes);
        }
    }
}
