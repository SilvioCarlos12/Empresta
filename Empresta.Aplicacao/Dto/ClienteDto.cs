namespace Empresta.Aplicacao.Dto
{
    public sealed record  ClienteDto(Guid ClienteId, string Nome, TelefoneDto Telefone, EnderecoDto Endereco) : 
        PessoaDto(Nome, Telefone, Endereco)
    {
    }
}
