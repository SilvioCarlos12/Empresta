using Empresta.Infraestrutura.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Empresta.Infraestrutura.DbContext
{
    public sealed class DbContext : IDbContext
    {
        private readonly IMongoDatabase _db;

        public DbContext(IOptions<MongoDbSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            _db = mongoClient.GetDatabase(options.Value.Database);

            var pingTask = _db.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
            if (!pingTask.Wait(TimeSpan.FromSeconds(5)))
            {
                throw new Exception("Database connection timed out.");
            }
            else if (pingTask.Result["ok"] != 1.0)
            {
                throw new Exception("Failed to connect to the database.");
            }
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            Type targetType = typeof(T);

            return _db.GetCollection<T>(targetType.Name);
        }

        public async Task<T> InsertDocument<T>(T payload, CancellationToken cancellationToken)
        {
            await GetCollection<T>().InsertOneAsync(payload, null, cancellationToken);

            return payload;
        }

        public async Task<T> UpdateDocument<T>(T payload, CancellationToken cancellationToken)
        {
            var filter = Builders<T>.Filter.Eq("_id",
                Guid.Parse(payload?.GetType()?.GetProperty("Id")?.GetValue(payload)?.ToString() ?? string.Empty));

            await GetCollection<T>().ReplaceOneAsync(filter, payload, new ReplaceOptions(), cancellationToken);

            return payload;
        }

        public async Task<T> DeleteDocument<T>(T payload, CancellationToken cancellationToken)
        {
            var filter = Builders<T>.Filter.Eq("_id",
                Guid.Parse(payload?.GetType()?.GetProperty("Id")?.GetValue(payload)?.ToString() ?? string.Empty));

            await GetCollection<T>().DeleteOneAsync(filter, cancellationToken);

            return payload;
        }
    }
}