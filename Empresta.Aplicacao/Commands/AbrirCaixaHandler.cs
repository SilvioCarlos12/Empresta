﻿using System.Text.Json.Serialization;
using Empresta.Aplicacao.Dto;
using Empresta.Aplicacao.MensagemErros;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using FluentValidation;
using MediatR;

namespace Empresta.Aplicacao.Commands;

public sealed class AbrirCaixaHandler(
    IFuncionarioRepositorio funcionarioRepositorio)
    : IRequestHandler<AbrirCaixaCommand, AbrirCaixaResponse>
{
    public async Task<AbrirCaixaResponse> Handle(AbrirCaixaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var funcionario = await funcionarioRepositorio.GetById(request.FuncionarioId, cancellationToken);

            if (funcionario is null)
            {
                return AbrirCaixaResponse.NaoEncontrado();
            }
            
            if (funcionario.ExisteCaixaAberto())
            {
                return AbrirCaixaResponse.Invalido(new ErroDto(CodigosErros.JaExisteCaixaEmAberto,
                    MensagensErro.JaExisteCaixaAberto));
            }

            funcionario.AbrirOCaixa(request.ValorInicial);

            await funcionarioRepositorio.Update(funcionario, cancellationToken);

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
    [JsonIgnore] public Guid FuncionarioId { get; set; }
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