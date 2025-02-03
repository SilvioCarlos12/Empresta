using Empresta.Aplicacao.Dto;
using Empresta.Dominio;

namespace Empresta.Aplicacao.Mapper
{
    public static class ClienteDtoMapper
    {
        public static Cliente ToEntidade(this ClienteDto dto)
        {
            return Cliente.Criar(dto.Nome, dto.Telefone.ToVo(), dto.Endereco.ToVo());
        }

        public static ClienteDto ToDto(this Cliente cliente)
        {
            return new ClienteDto(cliente.Nome, cliente.Telefone.ToDto(), cliente.Endereco.ToDto());
        }
    }
}
