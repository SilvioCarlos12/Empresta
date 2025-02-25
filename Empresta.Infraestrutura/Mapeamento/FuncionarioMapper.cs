using Empresta.Dominio;
using MongoDB.Bson.Serialization;

namespace Empresta.Infraestrutura.Mapeamento;

public static class FuncionarioMapper
{
    public static void Register()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Funcionario)))
        {
            BsonClassMap.RegisterClassMap<Funcionario>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapMember(p => p.Clientes).SetElementName("Clientes");
                cm.MapMember(c => c.Caixas).SetElementName("Caixas");
            });
        }
    }
}