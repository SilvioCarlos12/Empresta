using Empresta.Dominio;
using MongoDB.Bson.Serialization;

namespace Empresta.Infraestrutura.Mapeamento;

public static class ClienteMapper
{
    public static void Register()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Cliente)))
        {
            BsonClassMap.RegisterClassMap<Cliente>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}