namespace Empresta.Dominio.Vo
{
    public record Endereco
    {
        public Endereco(string rua, string bairro, string cidade, string cep, string estado, string numero)
        {
            Rua = rua;
            Bairro = bairro;
            Cidade = cidade;
            Cep = cep;
            Estado = estado;
            Numero = numero;
        }

        public string Rua { get; private set; } = string.Empty;
        public string Bairro { get; private set; } = string.Empty;
        public string Cidade { get; private set; } = string.Empty;
        public string Cep { get; private set; } = string.Empty;
        public string Estado { get; private set; } = string.Empty;
        public string Numero { get; private set; } = string.Empty;
    }
}
