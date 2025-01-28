using Empresta.Dominio.Vo;

namespace Empresta.Dominio
{
    public class Funcionario : Pessoa
    {
        public List<Cliente> Clientes { get; private set; } = new List<Cliente>();
        public Guid FuncionarioId { get; set; } = Guid.NewGuid();
        private Funcionario(string nome, Telefone telefone, Endereco endereco) : base(nome, telefone, endereco)
        {
        }
        
        public void AdicionarCliente(Cliente cliente)
        {
            Clientes.Add(cliente);
        }

        public static Funcionario Criar(string nome, Telefone telefone, Endereco endereco)
        {
            return new Funcionario(nome, telefone, endereco);
        }
    }
}
