using System.Text.Json.Serialization;
using Empresta.Aplicacao.Dto;
using Empresta.Aplicacao.MensagemErros;
using Empresta.Dominio;
using Empresta.Dominio.Enums;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using FluentValidation;
using MediatR;

namespace Empresta.Aplicacao.Commands;

public sealed class AbrirCaixaHandler : IRequestHandler<AbrirCaixaCommand,AbrirCaixaResponse>
{
    private readonly IFuncionarioRepositorio _funcionarioRepositorio;
    private readonly ICaixaRepositorio _caixaRepositorio;

    public AbrirCaixaHandler(IFuncionarioRepositorio funcionarioRepositorio, ICaixaRepositorio caixaRepositorio)
    {
        _funcionarioRepositorio = funcionarioRepositorio;
        _caixaRepositorio = caixaRepositorio;
    }

    public async Task<AbrirCaixaResponse> Handle(AbrirCaixaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var funcionario = await _funcionarioRepositorio.GetById(request.FuncionarioId, cancellationToken);

            if (funcionario is null)
            {
                return AbrirCaixaResponse.NaoEncontrado();
            }

            var existeEmAberto =
                await _caixaRepositorio.GetByFilter(x => x.StatusCaixa == StatusCaixa.Aberto, cancellationToken);

            if (existeEmAberto.Count != 0)
            {
                return AbrirCaixaResponse.Invalido(new ErroDto(CodigosErros.JaExisteCaixaEmAberto,
                    MensagensErro.JaExisteCaixaAberto));
            }
            
            var caixa = Caixa.AbrirCaixa(request.ValorInicial, request.FuncionarioId);

            await _caixaRepositorio.Add(caixa,cancellationToken);

            return AbrirCaixaResponse.Sucesso();
        }
        catch (Exception e)
        {
            return AbrirCaixaResponse.Erro(new ErroDto(CodigosErros.ErroSistematico, e.Message));
        }
    }
}

public sealed record AbrirCaixaCommand(decimal ValorInicial) : IRequest<AbrirCaixaResponse>
{
    [JsonIgnore]
    public Guid FuncionarioId { get; set; }
}

public sealed record AbrirCaixaSucesso() : AbrirCaixaResponse;
public sealed record AbrirCaixaNaoEncontrado() : AbrirCaixaResponse;
public sealed record AbrirCaixaInvalido(ErroDto ErroDto) : AbrirCaixaResponse;
public sealed record AbrirCaixaErro(ErroDto ErroDto) : AbrirCaixaResponse;
public record AbrirCaixaResponse
{
    public static AbrirCaixaResponse Sucesso() => new AbrirCaixaSucesso();
    public static AbrirCaixaResponse NaoEncontrado() => new AbrirCaixaNaoEncontrado();
    public static AbrirCaixaResponse Invalido(ErroDto erro) => new AbrirCaixaInvalido(erro); 
    public static AbrirCaixaResponse Erro(ErroDto erro) => new AbrirCaixaErro(erro);
}

public sealed class AbrirCaixaValidation : AbstractValidator<AbrirCaixaCommand>
{
    public AbrirCaixaValidation()
    {
        RuleFor(x => x.ValorInicial)
            .GreaterThan(0)
            .WithErrorCode(CodigosErros.ValorInicialEObrigatorio)
            .WithMessage(MensagensErro.ValorInicialEObrigatorio);
    }
}