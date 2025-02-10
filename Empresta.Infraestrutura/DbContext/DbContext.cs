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
            var clent = new MongoClient(options.Value.ConnectionString);
            _db = clent.GetDatabase(options.Value.Database);

            // Check if the database connection is successful
            var pingTask = _db.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
            if (!pingTask.Wait(TimeSpan.FromSeconds(5))) // Adjust timeout as needed
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

        // Common method for inserting document
        public async Task<T> InsertDocument<T>(T payload, CancellationToken cancellationToken)
        {
            await GetCollection<T>().InsertOneAsync(payload, null, cancellationToken);

            return payload;
        }

        //Common method for updating the document
        public async Task<T> UpdateDocument<T>(T payload, CancellationToken cancellationToken)
        {
            var filter = Builders<T>.Filter.Eq("_id",
                Guid.Parse(payload?.GetType()?.GetProperty("Id")?.GetValue(payload)?.ToString()));

            var updateResult = await GetCollection<T>().ReplaceOneAsync(filter, payload, new ReplaceOptions(), cancellationToken);

            return payload;
        }

        //Common method for delete the document
        public async Task<T> DeleteDocument<T>(T payload, CancellationToken cancellationToken)
        {
            var filter = Builders<T>.Filter.Eq("_id",
                ObjectId.Parse(payload?.GetType()?.GetProperty("Id")?.GetValue(payload)?.ToString()));

            var deleteResult = await GetCollection<T>().DeleteOneAsync(filter, cancellationToken);

            return payload;
        }
    }
}
