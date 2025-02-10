using Empresta.Aplicacao.Dto;

namespace Empresta.Aplicacao.Teste.Builds;

public sealed class ClienteDtoBuild
{
    private string _nome;
    private EnderecoDto _enderecoDto;
    private TelefoneDto _telefoneDto;

    public ClienteDtoBuild()
    {
        _nome = "Teste";
        _enderecoDto = new EnderecoDtoBuild().Build();
        _telefoneDto = new TelefoneDtoBuild().Buid();
    }

    public ClienteDtoBuild ComTelefone(TelefoneDto telefoneDto)
    {
        _telefoneDto = telefoneDto;
        return this;
    }
    
    
    public ClienteDtoBuild ComEndereco(EnderecoDto enderecoDto)
    {
        _enderecoDto = enderecoDto;
        return this;
    }

    public ClienteDtoBuild ComNome(string nome)
    {
        _nome = nome;
        return this;
    }

    public ClienteDto Build()
    {
        return new ClienteDto(_nome, _telefoneDto, _enderecoDto);
    }
}