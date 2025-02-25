using Empresta.Dominio.Enums;
using Empresta.Dominio.Vo;

namespace Empresta.Dominio;

public sealed class Caixa
{
    public Guid Id { get;  set; } = Guid.NewGuid();
    public StatusCaixa StatusCaixa { get; private set; }
    public DateTime DataAbertura { get; private set; }
    public DateTime? DataFechamento { get; private set; }
    public decimal ValorInicial { get; private set; }
    public List<FluxoCaixa> FluxoCaixas { get; private set; } = new List<FluxoCaixa>();

    public bool EstarAberto()
    {
        return StatusCaixa == StatusCaixa.Aberto;
    }

    private Caixa(decimal valorInicial)
    {
        ValorInicial = valorInicial;
        DataAbertura = DateTime.UtcNow;
        StatusCaixa = StatusCaixa.Aberto;
        CriarFluxoCaixa(valorInicial, TipoDespesa.AberturaDeCaixa);
    }

    public void FecharCaixa()
    {
        DataFechamento = DateTime.Now;
        StatusCaixa = StatusCaixa.Fechado;
    }

    public void CriarFluxoCaixa(decimal valor, TipoDespesa tipoDespesa)
    {
        var fluxoCaixa = FluxoCaixa.Criar(valor, tipoDespesa);

        FluxoCaixas.Add(fluxoCaixa);
    }

    public static Caixa AbrirCaixa(decimal valorDecimal)
    {
        return new Caixa(valorDecimal);
    }
}