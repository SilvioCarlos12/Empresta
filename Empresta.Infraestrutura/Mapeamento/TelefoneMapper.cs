using Empresta.Dominio.Vo;
using MongoDB.Bson.Serialization;

namespace Empresta.Infraestrutura.Mapeamento;

public static class TelefoneMapper
{
    public static void Register()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Telefone)))
        {
            BsonClassMap.RegisterClassMap<Telefone>(cm =>
            {
                cm.AutoMap();
            });
        }
    }
}