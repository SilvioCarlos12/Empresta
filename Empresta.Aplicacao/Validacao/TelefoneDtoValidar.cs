using Empresta.Aplicacao.Dto;
using Empresta.Aplicacao.MensagemErros;
using Empresta.Dominio.Vo;
using FluentValidation;

namespace Empresta.Aplicacao.Validacao
{
    public class TelefoneDtoValidar
    {
        public static TelefoneDtoValidacao ObterValidacao() => new();
    }

    public class TelefoneDtoValidacao : AbstractValidator<TelefoneDto>
    {
        public TelefoneDtoValidacao() {

            RuleFor(x => x.NumeroTelefone)
              .Must(Telefone.ValidarNumero)
              .WithErrorCode(CodigosErros.NumeroTelefoneInvalido)
              .WithMessage(MensagensErro.NumeroTelefoneInvalido);


            RuleFor(x => x.Dd)
             .Must(Telefone.ValidarDd)
             .WithErrorCode(CodigosErros.DdInvalido)
             .WithMessage(MensagensErro.DdInvalido);
        }
    }
}
