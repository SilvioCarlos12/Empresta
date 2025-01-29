namespace Empresta.Dominio.Vo
{
    public record Endereco
    {
        public Endereco()
        {
            
        }
        public Endereco(string rua, string bairro, string cidade, string cep, string estado, string numero)
        {
            Rua = rua;
            Bairro = bairro;
            Cidade = cidade;
            Cep = cep;
            Estado = estado;
            Numero = numero;
        }

        public string Rua { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
    }
}
