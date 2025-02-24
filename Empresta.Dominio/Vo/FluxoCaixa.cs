using Empresta.Dominio.Enums;

namespace Empresta.Dominio.Vo;

public record struct FluxoCaixa
{
    public decimal Valor { get; set; }
    public TipoDespesa TipoDespesa { get; set; }
    public DateTime DataCriacao { get; set; }

    private FluxoCaixa(decimal valor, TipoDespesa tipoDespesa)
    {
        Valor = valor;
        TipoDespesa = tipoDespesa;
        DataCriacao = DateTime.UtcNow;
    }

    public static FluxoCaixa Criar(decimal valor, TipoDespesa tipoDespesa)
    {
        return new FluxoCaixa(valor, tipoDespesa);
    }
}