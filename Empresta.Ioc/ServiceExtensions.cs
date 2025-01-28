using Empresta.Infraestrutura.DbContext;
using Empresta.Infraestrutura.Mapeamento;
using Empresta.Infraestrutura.Repositorios;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using Empresta.Infraestrutura.Settings;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;


namespace Empresta.Ioc;

public static class ServiceExtensions
{
    public static IServiceCollection AddConfigMongo(this IServiceCollection serviceCollection,
        string conectionString, string dataBase)
    {
        serviceCollection.Configure<MongoDbSettings>(
            options =>
            {
                options.ConnectionString = conectionString;
                options.Database = dataBase;

            });
        return serviceCollection;
    }
    public static IServiceCollection AddRepositorio(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IClienteRepositorio, ClienteRepositorio>();
        serviceCollection.AddScoped<IFuncionarioRepositorio, FuncionarioRepositorio>();
        
        return serviceCollection;
    }
    public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDbContext, DbContext>();
        return serviceCollection;
    }

    public static void AddMapeamento(this IServiceCollection serviceCollection)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        TelefoneMapper.Register();
        EnderecoMapper.Register();
        ClienteMapper.Register();
        FuncionarioMapper.Register();
    }
}