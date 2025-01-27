using Bogus;
using Empresta.Dominio.Vo;

namespace Empresta.Dominio.Teste.Builds
{
    public class EnderecoBuild
    {
        private Endereco _endereco;

        public EnderecoBuild()
        {
            _endereco = new Faker<Endereco>("pt_BR")
                              .RuleFor(x => x.Cep, faker => faker.Address.ZipCode())
                              .RuleFor(x => x.Cidade, faker => faker.Address.City())
                              .RuleFor(x => x.Rua, faker => faker.Address.StreetName())
                              .RuleFor(x => x.Numero, faker => faker.Address.BuildingNumber())
                              .RuleFor(x => x.Estado, faker => faker.Address.Country())
                              .Generate();
        }

        public EnderecoBuild ComCep(string cep)
        {
            _endereco.Cep = cep;
            return this;
        }

        public EnderecoBuild ComCidade(string cidade)
        {
            _endereco.Cidade = cidade;
            return this;
        }

        public EnderecoBuild ComRua(string rua)
        {
            _endereco.Rua = rua;
            return this;
        }

        public EnderecoBuild ComNumero(string numero)
        {
            _endereco.Numero = numero;
            return this;
        }

        public EnderecoBuild ComEstado(string estado)
        {
            _endereco.Estado = estado;
            return this;
        }

        public Endereco Build()
        {
            return _endereco;
        }

    }
}
