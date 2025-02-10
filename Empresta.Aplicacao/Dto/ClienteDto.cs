namespace Empresta.Aplicacao.Dto
{
    public sealed record class ClienteDto : PessoaDto
    {
        public ClienteDto(string Nome, TelefoneDto Telefone, EnderecoDto Endereco) : base(Nome, Telefone, Endereco)
        {
        }
    }
}
