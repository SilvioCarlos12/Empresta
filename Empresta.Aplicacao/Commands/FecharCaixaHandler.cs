using Empresta.Aplicacao.Dto;
using Empresta.Aplicacao.MensagemErros;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using FluentValidation;
using MediatR;

namespace Empresta.Aplicacao.Commands;

public sealed class FecharCaixaHandler(IFuncionarioRepositorio funcionarioRepositorio)
    : IRequestHandler<FecharCaixaCommand, FecharCaixaResponse>
{
    public async Task<FecharCaixaResponse> Handle(FecharCaixaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var funcionario = await funcionarioRepositorio.GetById(request.FuncionarioId, cancellationToken);

            if (funcionario is null)
            {
                return FecharCaixaResponse.NaoEncontrado();
            }

            if (funcionario.ExisteCaixaAberto())
            {
                
                funcionario.FecharOCaixa();

                await funcionarioRepositorio.Update(funcionario, cancellationToken);
                return FecharCaixaResponse.Sucesso();
            }

            return new FecharCaixaInvalido(new ErroDto(CodigosErros.NaoHaCaixaAberto,
                MensagensErro.NaoHaCaixaAberto));
        }
        catch (Exception e)
        {
            return FecharCaixaResponse.Erro(new ErroDto(CodigosErros.ErroSistematico, e.Message));
        }
    }
}

public sealed record FecharCaixaCommand(Guid FuncionarioId) : IRequest<FecharCaixaResponse>;

public sealed record FecharCaixaSucesso() : FecharCaixaResponse;

public sealed record FecharCaixaInvalido(ErroDto ErroDto) : FecharCaixaResponse;

public sealed record FecharCaixaNaoEncontrado() : FecharCaixaResponse;

public sealed record FecharCaixaErro(ErroDto ErroDto) : FecharCaixaResponse;

public record FecharCaixaResponse
{
    public static FecharCaixaResponse Sucesso() => new FecharCaixaSucesso();
    public static FecharCaixaResponse NaoEncontrado() => new FecharCaixaNaoEncontrado();
    public static FecharCaixaResponse Invalido(ErroDto erro) => new FecharCaixaInvalido(erro);
    public static FecharCaixaResponse Erro(ErroDto erro) => new FecharCaixaErro(erro);
}

public sealed class FecharCaixaValidation : AbstractValidator<FecharCaixaCommand>
{
    
}