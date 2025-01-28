using Empresta.Dominio.Vo;

namespace Empresta.Dominio
{
    public abstract class Pessoa
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        protected Pessoa(string nome, Telefone telefone, Endereco endereco)
        {
            Nome = nome;
            Telefone = telefone;
            Endereco = endereco;
        }

        public string Nome { get; set; }
        public Telefone Telefone { get; set; }
        public Endereco Endereco { get; set; }
    }
}
