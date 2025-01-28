using MongoDB.Driver;

namespace Empresta.Infraestrutura.DbContext
{
    public interface IDbContext
    {
        IMongoCollection<T> GetCollection<T>();
        public Task<T> InsertDocument<T>(T payload, CancellationToken cancellationToken);
        public Task<T> UpdateDocument<T>(T payload, CancellationToken cancellationToken);
        public Task<T> DeleteDocument<T>(T payload, CancellationToken cancellationToken);
    }
}
