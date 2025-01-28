using Empresta.Dominio.Vo;
using MongoDB.Bson.Serialization;

namespace Empresta.Infraestrutura.Mapeamento;

public static class EnderecoMapper
{
    public static void Register()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Endereco)))
        {
            BsonClassMap.RegisterClassMap<Endereco>(cm =>
            {
                cm.AutoMap();
            });
        }
    }
}