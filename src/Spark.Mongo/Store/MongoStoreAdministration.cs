namespace Spark.Mongo.Store
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Spark.Engine.Interfaces;

    using Search.Infrastructure;

    public class MongoStoreAdministration : IFhirStoreAdministration
    {
        readonly IMongoDatabase database;
        readonly IMongoCollection<BsonDocument> collection;

        public MongoStoreAdministration(string mongoUrl)
        {
            this.database = MongoDatabaseFactory.GetMongoDatabase(mongoUrl);
            this.collection = database.GetCollection<BsonDocument>(Collection.RESOURCE);
        }
        
        public async Task Clean()
        {
            await EraseDataAsync().ConfigureAwait(false);
            await EnsureIndicesAsync().ConfigureAwait(false);
        }

        // Drops all collections, including the special 'counters' collection for generating ids,
        // AND the binaries stored at Amazon S3
        private async Task EraseDataAsync()
        {
            // Don't try this at home
            var collectionsToDrop = new string[] { Collection.RESOURCE, Collection.COUNTERS, Collection.SNAPSHOT };
            await DropCollectionsAsync(collectionsToDrop).ConfigureAwait(false);

            /*
            // When using Amazon S3, remove blobs from there as well
            if (Config.Settings.UseS3)
            {
                using (var blobStorage = getBlobStorage())
                {
                    if (blobStorage != null)
                    {
                        blobStorage.Open();
                        blobStorage.DeleteAll();
                        blobStorage.Close();
                    }
                }
            }
            */
        }
        private async Task DropCollectionsAsync(IEnumerable<string> collections)
        {
            foreach (var name in collections)
            {
                await TryDropCollectionAsync(name).ConfigureAwait(false);
            }
        }



        private async Task EnsureIndicesAsync()
        {
            var indices = new List<CreateIndexModel<BsonDocument>>
            {
                new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Ascending(Field.STATE).Ascending(Field.METHOD).Ascending(Field.TYPENAME)),
                new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Ascending(Field.PRIMARYKEY).Ascending(Field.STATE)),
                new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Descending(Field.WHEN).Ascending(Field.TYPENAME)),
            };
            await collection.Indexes.CreateManyAsync(indices).ConfigureAwait(false);
        }

        private async Task TryDropCollectionAsync(string name)
        {
            try
            {
                await database.DropCollectionAsync(name).ConfigureAwait(false);
            }
            catch
            {
                //don't worry. if it's not there. it's not there.
            }
        }
    }
}