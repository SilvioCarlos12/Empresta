using Empresta.Aplicacao.Dto;
using Empresta.Dominio.Vo;

namespace Empresta.Aplicacao.Mapper
{
    public static class TelefoneDtoMapper
    {
        public static Telefone ToVo(this TelefoneDto telefone)
        {
            return new Telefone(telefone.Dd, telefone.NumeroTelefone);
        }
    }
}
