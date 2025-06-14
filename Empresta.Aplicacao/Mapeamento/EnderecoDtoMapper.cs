﻿using Empresta.Aplicacao.Dto;
using Empresta.Dominio.Vo;

namespace Empresta.Aplicacao.Mapper
{
    public static class EnderecoDtoMapper
    {
        public static Endereco ToVo(this EnderecoDto enderecoDto)
        {
            return new Endereco(
                enderecoDto.Rua, 
                enderecoDto.Bairro, 
                enderecoDto.Cidade, 
                enderecoDto.Cep, 
                enderecoDto.Estado, 
                enderecoDto.Numero);
        }

        public static EnderecoDto ToDto(this Endereco Vo)
        {
            return new EnderecoDto(Vo.Rua,Vo.Bairro,Vo.Cidade,Vo.Cep,Vo.Estado,Vo.Numero);
        }
    }
}
