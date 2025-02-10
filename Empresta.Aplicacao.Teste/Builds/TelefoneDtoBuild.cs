using Empresta.Aplicacao.Dto;

namespace Empresta.Aplicacao.Teste.Builds;

public sealed class TelefoneDtoBuild
{
    private string _dd;
    private string _numeroTelefone;

    public TelefoneDtoBuild()
    {
        _dd = "098";
        _numeroTelefone = "987246527";
    }

    public TelefoneDtoBuild ComDd(string dd)
    {
        _dd = dd;
        return this;
    }

    public TelefoneDtoBuild ComNumeroTelefone(string telefone)
    {
        _numeroTelefone = telefone;
        return this;
    }

    public TelefoneDto Buid()
    {
        return new TelefoneDto(_dd, _numeroTelefone);
    }
}