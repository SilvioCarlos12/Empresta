namespace Empresta.Dominio.Vo
{
    public sealed record Telefone
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

        public static bool ValidarDd(string dd)
        {
            return dd.Length == 3 && int.TryParse(dd, out var resultado);
        }
        public static bool ValidarNumero(string numero)
        {
            return numero.Length == 9 && int.TryParse(numero, out var resultado);
        }
        public string TelefoneCompleto()
        {
            return Dd + Numero;
        }
    }
}
