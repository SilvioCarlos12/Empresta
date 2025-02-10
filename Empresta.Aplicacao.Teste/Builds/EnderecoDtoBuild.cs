using Empresta.Aplicacao.Dto;

namespace Empresta.Aplicacao.Teste.Builds;

public sealed class EnderecoDtoBuild
{
    private string _rua;
    private string _bairro;
    private string _cidade;
    private string _cep;
    private string _estado;
    private string _numero;

    public EnderecoDtoBuild()
    {
        _rua = "Rua 12";
        _bairro = "Bairro 13";
        _cidade = "Cidade 14";
        _cep = "65040430";
        _estado = "Estado 15";
        _numero = "123";
    }
    
    public EnderecoDtoBuild ComCep(string cep)
    {
        _cep = cep;
        return this;
    }

    public EnderecoDtoBuild ComCidade(string cidade)
    {
        _cidade = cidade;
        return this;
    }

    public EnderecoDtoBuild ComRua(string rua)
    {
        _rua = rua;
        return this;
    }

    public EnderecoDtoBuild ComNumero(string numero)
    {
        _numero = numero;
        return this;
    }

    public EnderecoDtoBuild ComEstado(string estado)
    {
        _estado = estado;
        return this;
    }
    
    public EnderecoDtoBuild ComBairro(string bairro)
    {
        _bairro = bairro;
        return this;
    }

    public EnderecoDto Build()
    {
        return new EnderecoDto(_rua,_bairro,_cidade,_cep,_estado,_numero);
    }

}