﻿using Empresta.Dominio.Vo;

namespace Empresta.Dominio
{
    public sealed class Cliente : Pessoa
    {
        private Cliente(string nome, Telefone telefone, Endereco endereco) : base(nome, telefone, endereco)
        {
        }

        public void AtualizarCliente(string nome, Telefone telefone, Endereco endereco)
        {
            Nome = nome;
            Telefone = telefone;
            Endereco = endereco;
        }
        public static Cliente Criar(string nome, Telefone telefone, Endereco endereco)
        {
            return new Cliente(nome, telefone, endereco);
        }
    }
}
