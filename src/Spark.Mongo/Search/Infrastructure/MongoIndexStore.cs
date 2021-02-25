using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Spark.Engine.Core;
using System.Threading.Tasks;
using Spark.Store.Mongo;
using Spark.Engine.Model;
using Spark.Mongo.Search.Indexer;
using Spark.Engine.Store.Interfaces;

namespace Spark.Mongo.Search.Common
{
    using Engine.Extensions;
    using Infrastructure;

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
            await Collection.DeleteManyAsync(query).ConfigureAwait(false);
            await Collection.InsertOneAsync(document).ConfigureAwait(false);
        }

        public async Task Delete(Entry entry)
        {
            string id = entry.Key.WithoutVersion().ToOperationPath();
            var query = Builders<BsonDocument>.Filter.Eq(InternalField.ID, id);
            await Collection.DeleteManyAsync(query).ConfigureAwait(false);
        }

        public async Task Clean()
        {
            await Collection.DeleteManyAsync(Builders<BsonDocument>.Filter.Empty).ConfigureAwait(false);
        }
    }
}
