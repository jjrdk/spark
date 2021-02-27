// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Mongo.Store
{
    using System.Threading.Tasks;
    using Engine.Core;
    using Engine.Store.Interfaces;
    using MongoDB.Driver;
    using Search.Infrastructure;

    public class MongoSnapshotStore : ISnapshotStore
    {
        private readonly IMongoDatabase _database;

        public MongoSnapshotStore(string mongoUrl) => _database = MongoDatabaseFactory.GetMongoDatabase(mongoUrl);

        public async Task AddSnapshot(Snapshot snapshot)
        {
            var collection = _database.GetCollection<Snapshot>(Collection.SNAPSHOT);
            await collection.InsertOneAsync(snapshot).ConfigureAwait(false);
        }

        public async Task<Snapshot> GetSnapshot(string snapshotId)
        {
            var collection = _database.GetCollection<Snapshot>(Collection.SNAPSHOT);
            return (await collection.FindAsync(s => s.Id == snapshotId).ConfigureAwait(false)).FirstOrDefault();
        }
    }
}