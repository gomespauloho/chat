using MongoDB.Driver;

namespace Chat.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity>
    {
        protected readonly IMongoCollection<TEntity> Collection;

        /*public BaseRepository(IOptions<DatabaseSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.DatabaseName);

            Collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }*/

        public BaseRepository(IMongoDatabase mongoDatabase)
        {
            Collection = mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name);
        }
    }
}
