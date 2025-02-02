using Empresta.Aplicacao.Dto;
using Empresta.Aplicacao.MensagemErros;
using FluentValidation;

namespace Empresta.Aplicacao.Validacao
{
    public class EnderecoDtoValidar
    {
        public static EnderecoDtoValidaticao ObterValidacao() => new();
    }

    public class EnderecoDtoValidaticao : AbstractValidator<EnderecoDto>
    {
        public EnderecoDtoValidaticao()
        {

            RuleFor(x => x.Rua)
                 .NotEmpty()
                 .WithErrorCode(CodigosErros.RuaEObrigatorio)
                 .WithMessage(MensagensErro.RuaEObrigatorio);


            RuleFor(x => x.Bairro)
                .NotEmpty()
                .WithErrorCode(CodigosErros.BairroEObrigatorio)
                .WithMessage(MensagensErro.BairroEObrigatorio);


            RuleFor(x => x.Estado)
                .NotEmpty()
                .WithErrorCode(CodigosErros.EstadoEObrigatorio)
                .WithMessage(MensagensErro.EstadoEObrigatorio);


            RuleFor(x => x.Cidade)
                .NotEmpty()
                .WithErrorCode(CodigosErros.CidadeEObrigatorio)
                .WithMessage(MensagensErro.CidadeEObrigatorio);


            RuleFor(x => x.Numero)
                .NotEmpty()
                .WithErrorCode(CodigosErros.NumeroEObrigatorio)
                .WithMessage(MensagensErro.NumeroEObrigatorio);


            RuleFor(x => x.Cep)
                .NotEmpty()
                .WithErrorCode(CodigosErros.CepEObrigatorio)
                .WithMessage(MensagensErro.CepEObrigatorio);
        }
    }
}
