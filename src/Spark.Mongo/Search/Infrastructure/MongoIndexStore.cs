﻿namespace Spark.Mongo.Search.Infrastructure
{
    using System.Threading.Tasks;
    using Common;
    using Engine.Core;
    using Engine.Extensions;
    using Engine.Model;
    using Engine.Store.Interfaces;
    using Indexer;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class MongoIndexStore : IIndexStore
    {
        private readonly IMongoDatabase _database;
        private readonly MongoIndexMapper _indexMapper;
        public IMongoCollection<BsonDocument> Collection;

        public MongoIndexStore(string mongoUrl, MongoIndexMapper indexMapper)
        {
            _database = MongoDatabaseFactory.GetMongoDatabase(mongoUrl);
            _indexMapper = indexMapper;
            Collection = _database.GetCollection<BsonDocument>(Config.MONGOINDEXCOLLECTION);
        }

        public async Task Save(IndexValue indexValue)
        {
            var result = _indexMapper.MapEntry(indexValue);

            foreach (var doc in result)
            {
                await Save(doc).ConfigureAwait(false);
            }
        }

        public async Task Save(BsonDocument document)
        {
            var keyvalue = document.GetValue(InternalField.ID).ToString();
                var query = Builders<BsonDocument>.Filter.Eq(InternalField.ID, keyvalue);

                // todo: should use Update: collection.Update();
                _ = await Collection.DeleteManyAsync(query).ConfigureAwait(false);
                await Collection.InsertOneAsync(document).ConfigureAwait(false);
        }

        public Task Delete(Entry entry)
        {
            var id = entry.Key.WithoutVersion().ToOperationPath();
            var query = Builders<BsonDocument>.Filter.Eq(InternalField.ID, id);
            return Collection.DeleteManyAsync(query);
        }

        public Task Clean()
        {
            return Collection.DeleteManyAsync(Builders<BsonDocument>.Filter.Empty);
        }

    }
}
