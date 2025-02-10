using Bogus;
using Empresta.Dominio.Vo;

namespace Empresta.Dominio.Teste.Builds
{
    public sealed class TelefoneBuild
    {
        private Telefone _telefone;

        public TelefoneBuild()
        {

            _telefone = new Faker<Telefone>("pt_BR")
                               .RuleFor(x => x.Dd, faker => faker.Phone.PhoneNumberFormat())
                               .RuleFor(x => x.Numero, faker => faker.Phone.PhoneNumber())
                               .Generate(); 

        }

        public TelefoneBuild ComNumero(string numero)
        {
            _telefone.Numero = numero;
            return this;
        }

        public TelefoneBuild ComDd(string dd)
        {
            _telefone.Dd = dd;
            return this;
        }

        public Telefone Build()
        {
            return _telefone;
        }
    }
}
