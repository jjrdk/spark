using System;
using Hl7.Fhir.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Spark.Mongo.Store
{
    using System.Threading.Tasks;
    using Engine.Interfaces;
    using Search.Infrastructure;

    public class MongoIdGenerator : IGenerator
    {
        private readonly IMongoDatabase _database;

        public MongoIdGenerator(string mongoUrl)
        {
            this._database = MongoDatabaseFactory.GetMongoDatabase(mongoUrl);
        }
        async Task<string> IGenerator.NextResourceId(Resource resource)
        {
            var id = await Next(resource.TypeName).ConfigureAwait(false);
            return id;
        }

        Task<string> IGenerator.NextVersionId(string resourceIdentifier)
        {
            throw new NotImplementedException();
        }

        async Task<string> IGenerator.NextVersionId(string resourceType, string resourceIdentifier)
        {
            var name = resourceType + "_history_" + resourceIdentifier;
            var versionId = await Next(name).ConfigureAwait(false);
            return versionId;
        }

        private async Task<string> Next(string name)
        {
            var collection = _database.GetCollection<BsonDocument>(Collection.COUNTERS);

            var query = Builders<BsonDocument>.Filter.Eq(Field.PRIMARYKEY, name);
            var update = Builders<BsonDocument>.Update.Inc(Field.COUNTERVALUE, 1);
            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After,
                Projection = Builders<BsonDocument>.Projection.Include(Field.COUNTERVALUE)
            };
            var document = await collection.FindOneAndUpdateAsync(query, update, options).ConfigureAwait(false);

            var value = document[Field.COUNTERVALUE].AsInt32.ToString();
            return value;
        }
    }
}