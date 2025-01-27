using Empresta.Dominio.Teste.Builds;
using Xunit;

namespace Empresta.Dominio.Teste
{
    public class FuncionarioTeste
    {
        public FuncionarioTeste() { }

        [Fact(DisplayName ="Funcinario deve ser criado")]
        public void DeveCriarFuncionario()
        {
            //Arrange
            var nome = "Teste";
            var telefone = new TelefoneBuild().Build();
            var endereco = new EnderecoBuild().Build();

            //Act

            var funcionario = new FuncionarioBuild()
            .ComNome(nome)
            .ComEndereco(endereco)
            .ComTelefone(telefone)
            .Build();

            //Assert

            Assert.Equal(funcionario.Nome, nome);
            Assert.Equal(funcionario.Telefone, telefone);
            Assert.Equal(funcionario.Endereco, endereco);
        }
    }
}
