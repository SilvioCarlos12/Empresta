using Empresta.Dominio.Enums;
using Empresta.Dominio.Vo;

namespace Empresta.Dominio
{
    public sealed class Funcionario : Pessoa
    {
        public List<Cliente> Clientes { get; private set; } = new List<Cliente>();
        public List<Caixa> Caixas { get; private set; } = new List<Caixa>();
        private Funcionario(string nome, Telefone telefone, Endereco endereco) : base(nome, telefone, endereco)
        {
        }
        
        public void AdicionarCliente(Cliente cliente)
        {
            Clientes.Add(cliente);
        }

        public bool ExisteCaixaAberto()
        {
            return Caixas is not null && Caixas.Exists(x => x.EstarAberto());
        }

        public void AbrirOCaixa(decimal valorInicial)
        {
            var caixa = Caixa.AbrirCaixa(valorInicial);
            
            if (Caixas is null)
            {
                Caixas = [];
            }
            
            Caixas.Add(caixa);
        }

        public void FecharOCaixa()
        {
            var caixaAberto = Caixas.Single(x => x.EstarAberto());

            Caixas.Remove(caixaAberto);
            
            caixaAberto.FecharCaixa();
            
            Caixas.Add(caixaAberto);
        }

        public void AdicionarFluxoCaixa(decimal valor , TipoDespesa tipoDespesa)
        {
            var caixaAberto = Caixas.Single(x => x.EstarAberto());

            caixaAberto.CriarFluxoCaixa(valor,tipoDespesa);

            AtualizarCaixa(caixaAberto);
        }

        public void AtualizarCaixa(Caixa caixa)
        {
            Caixas.Remove(caixa);
            Caixas.Add(caixa);
        }
        
        public void AtualizarClienteDoFuncionario(Cliente cliente)
        {
            Clientes.Remove(cliente);

            Clientes.Add(cliente);
        }

        public static Funcionario Criar(string nome, Telefone telefone, Endereco endereco)
        {
            return new Funcionario(nome, telefone, endereco);;
        }
    }
}
