namespace Empresta.Dominio.Vo
{
    public record Telefone
    {
        public string Dd { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;

        public Telefone()
        {
            
        }
        public Telefone(string dd, string numero)
        {

            Dd = dd;
            Numero = numero;
        }

        public string TelefoneCompleto()
        {
            return Dd + Numero;
        }
    }
}
