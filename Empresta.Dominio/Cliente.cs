using Empresta.Dominio.Vo;

namespace Empresta.Dominio
{
    public class Cliente : Pessoa
    {
        private Cliente(string nome, Telefone telefone, Endereco endereco) : base(nome, telefone, endereco)
        {
        }

        public static Cliente Criar(string nome, Telefone telefone, Endereco endereco)
        {
            return new Cliente(nome, telefone, endereco);
        }
    }
}
