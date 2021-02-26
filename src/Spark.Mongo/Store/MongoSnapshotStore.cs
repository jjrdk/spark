namespace Spark.Mongo.Store
{
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using Spark.Engine.Core;
    using Spark.Engine.Store.Interfaces;
    using Search.Infrastructure;

    public class MongoSnapshotStore : ISnapshotStore
    {
        readonly IMongoDatabase database;

        public MongoSnapshotStore(string mongoUrl)
        {
            this.database = MongoDatabaseFactory.GetMongoDatabase(mongoUrl);
        }

        public async Task AddSnapshot(Snapshot snapshot)
        {
            var collection = database.GetCollection<Snapshot>(Collection.SNAPSHOT);
            await collection.InsertOneAsync(snapshot).ConfigureAwait(false);
        }

        public async Task<Snapshot> GetSnapshot(string snapshotId)
        {
            var collection = database.GetCollection<Snapshot>(Collection.SNAPSHOT);
            return (await collection.FindAsync(s => s.Id == snapshotId).ConfigureAwait(false)).FirstOrDefault();
        }
    }
}
