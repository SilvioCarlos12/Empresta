using Empresta.Aplicacao.Dto;

namespace Empresta.Aplicacao.Teste.Builds;

public sealed class ClienteDtoBuild
{
    private string _nome;
    private EnderecoDto _enderecoDto;
    private TelefoneDto _telefoneDto;
    private Guid _id;

    public ClienteDtoBuild()
    {
        _nome = "Teste";
        _id = Guid.NewGuid();
        _enderecoDto = new EnderecoDtoBuild().Build();
        _telefoneDto = new TelefoneDtoBuild().Build();
    }

    public ClienteDtoBuild ComTelefone(TelefoneDto telefoneDto)
    {
        _telefoneDto = telefoneDto;
        return this;
    }

    public ClienteDtoBuild ComId(Guid id)
    {
        _id = id;
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
        return new ClienteDto(_id, _nome, _telefoneDto, _enderecoDto);
    }
}