namespace Empresta.Dominio.Vo
{
    public record Telefone
    {
        public string Dd {  get; private set; } = string.Empty;
        public string Numero { get; private set; } = string.Empty;
        public Telefone(string dd,string numero) { 
          
            Dd = dd;
            Numero = numero;
        }
    }
}
