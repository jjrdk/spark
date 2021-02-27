// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Mongo.Store
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Engine.Interfaces;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Search.Infrastructure;

    public class MongoStoreAdministration : IFhirStoreAdministration
    {
        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly IMongoDatabase _database;

        public MongoStoreAdministration(string mongoUrl)
        {
            _database = MongoDatabaseFactory.GetMongoDatabase(mongoUrl);
            _collection = _database.GetCollection<BsonDocument>(Collection.RESOURCE);
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
            var collectionsToDrop = new[] {Collection.RESOURCE, Collection.COUNTERS, Collection.SNAPSHOT};
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
                new CreateIndexModel<BsonDocument>(
                    Builders<BsonDocument>.IndexKeys.Ascending(Field.STATE)
                        .Ascending(Field.METHOD)
                        .Ascending(Field.TYPENAME)),
                new CreateIndexModel<BsonDocument>(
                    Builders<BsonDocument>.IndexKeys.Ascending(Field.PRIMARYKEY).Ascending(Field.STATE)),
                new CreateIndexModel<BsonDocument>(
                    Builders<BsonDocument>.IndexKeys.Descending(Field.WHEN).Ascending(Field.TYPENAME))
            };
            await _collection.Indexes.CreateManyAsync(indices).ConfigureAwait(false);
        }

        private async Task TryDropCollectionAsync(string name)
        {
            try
            {
                await _database.DropCollectionAsync(name).ConfigureAwait(false);
            }
            catch
            {
                //don't worry. if it's not there. it's not there.
            }
        }
    }
}