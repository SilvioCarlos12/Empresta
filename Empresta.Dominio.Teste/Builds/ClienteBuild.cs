using Empresta.Dominio.Vo;

namespace Empresta.Dominio.Teste.Builds
{
    public class ClienteBuild
    {
        private string _nome;
        private Telefone _telefone;
        private Endereco _endereco;

        public ClienteBuild()
        {
            _nome = "Teste";
            _telefone = new TelefoneBuild().Build();
            _endereco = new EnderecoBuild().Build();
        }

        public ClienteBuild ComEndereco(Endereco endereco)
        {
            _endereco = endereco;
            return this;
        }

        public ClienteBuild ComTelefone(Telefone telefone)
        {
            _telefone = telefone;
            return this;
        }

        public ClienteBuild ComNome(string nome)
        {
            _nome = nome;
            return this;
        }

        public Cliente Build()
        {
            var cliente = Cliente.Criar(_nome, _telefone, _endereco);
            return cliente;
        }
    }
}
