using Empresta.Dominio;
using MongoDB.Bson.Serialization;

namespace Empresta.Infraestrutura.Mapeamento;

public static class CaixaMapper
{
    public static void Register()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Caixa)))
        {
            BsonClassMap.RegisterClassMap<Caixa>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}