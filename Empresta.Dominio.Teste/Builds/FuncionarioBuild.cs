using Empresta.Dominio.Vo;

namespace Empresta.Dominio.Teste.Builds
{
    public sealed class FuncionarioBuild
    {
        private string _nome;
        private Telefone _telefone;
        private Endereco _endereco;

        public FuncionarioBuild()
        {
            _nome = "Teste";
            _telefone = new TelefoneBuild().Build();
            _endereco = new EnderecoBuild().Build();
        }

        public FuncionarioBuild ComEndereco(Endereco endereco)
        {
            _endereco = endereco;
            return this;
        }

        public FuncionarioBuild ComTelefone(Telefone telefone)
        {
            _telefone = telefone;
            return this;
        }

        public FuncionarioBuild ComNome(string nome)
        {
            _nome = nome;
            return this;
        }

        public Funcionario Build()
        {
            var funcionario = Funcionario.Criar(_nome, _telefone, _endereco);
            return funcionario;
        }
    }
}
