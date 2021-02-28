// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Mongo.Store
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Engine.Core;
    using Engine.Extensions;
    using Engine.Store.Interfaces;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Search.Infrastructure;

    public class MongoFhirStore : IFhirStore
    {
        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly IMongoDatabase _database;

        public MongoFhirStore(string mongoUrl)
        {
            _database = MongoDatabaseFactory.GetMongoDatabase(mongoUrl);
            _collection = _database.GetCollection<BsonDocument>(Collection.RESOURCE);
            //this.transaction = new MongoSimpleTransaction(collection);
        }

        public async Task Add(Entry entry)
        {
            var document = SparkBsonHelper.ToBsonDocument(entry);
            await SupercedeAsync(entry.Key).ConfigureAwait(false);
            await _collection.InsertOneAsync(document).ConfigureAwait(false);
        }

        public async Task<Entry> Get(IKey key)
        {
            var clauses = new List<FilterDefinition<BsonDocument>>();

            clauses.Add(Builders<BsonDocument>.Filter.Eq(Field.TYPENAME, key.TypeName));
            clauses.Add(Builders<BsonDocument>.Filter.Eq(Field.RESOURCEID, key.ResourceId));

            if (key.HasVersionId())
            {
                clauses.Add(Builders<BsonDocument>.Filter.Eq(Field.VERSIONID, key.VersionId));
            }
            else
            {
                clauses.Add(Builders<BsonDocument>.Filter.Eq(Field.STATE, Value.CURRENT));
            }

            var query = Builders<BsonDocument>.Filter.And(clauses);

            var document = (await _collection.FindAsync(query).ConfigureAwait(false)).FirstOrDefault();
            return document.ToEntry();
        }

        public async Task<IList<Entry>> Get(IEnumerable<IKey> identifiers)
        {
            if (!identifiers.Any())
            {
                return new List<Entry>();
            }

            IList<IKey> identifiersList = identifiers.ToList();
            var versionedIdentifiers = GetBsonValues(identifiersList, k => k.HasVersionId());
            var unversionedIdentifiers = GetBsonValues(identifiersList, k => k.HasVersionId() == false);

            var queries = new List<FilterDefinition<BsonDocument>>();
            if (versionedIdentifiers.Any())
            {
                queries.Add(GetSpecificVersionQuery(versionedIdentifiers));
            }

            if (unversionedIdentifiers.Any())
            {
                queries.Add(GetCurrentVersionQuery(unversionedIdentifiers));
            }

            var query = Builders<BsonDocument>.Filter.Or(queries);

            var cursor = (await _collection.FindAsync(query).ConfigureAwait(false)).ToEnumerable();

            return cursor.ToEntries().ToList();
        }

        /// <inheritdoc />
        public async Task<bool> Exists(IKey key)
        {
            var clauses = new List<FilterDefinition<BsonDocument>>();
            var bsonValue = (BsonValue)key.ToString();
            if (key.HasVersionId())
            {
                clauses.Add(Builders<BsonDocument>.Filter.Eq(Field.PRIMARYKEY, bsonValue));
            }
            else
            {
                clauses.Add(Builders<BsonDocument>.Filter.Eq(Field.REFERENCE, bsonValue));
                clauses.Add(Builders<BsonDocument>.Filter.Eq(Field.STATE, Value.CURRENT));
            }

            var result = await _collection.CountDocumentsAsync(Builders<BsonDocument>.Filter.And(clauses))
                .ConfigureAwait(false);
            return result > 0;
        }

        private IEnumerable<BsonValue> GetBsonValues(IEnumerable<IKey> identifiers, Func<IKey, bool> keyCondition)
        {
            return identifiers.Where(keyCondition).Select(k => (BsonValue)k.ToString());
        }

        private FilterDefinition<BsonDocument> GetCurrentVersionQuery(IEnumerable<BsonValue> ids)
        {
            var clauses = new List<FilterDefinition<BsonDocument>>();
            clauses.Add(Builders<BsonDocument>.Filter.In(Field.REFERENCE, ids));
            clauses.Add(Builders<BsonDocument>.Filter.Eq(Field.STATE, Value.CURRENT));
            return Builders<BsonDocument>.Filter.And(clauses);
        }

        private FilterDefinition<BsonDocument> GetSpecificVersionQuery(IEnumerable<BsonValue> ids)
        {
            var clauses = new List<FilterDefinition<BsonDocument>>();
            clauses.Add(Builders<BsonDocument>.Filter.In(Field.PRIMARYKEY, ids));

            return Builders<BsonDocument>.Filter.And(clauses);
        }

        private async Task SupercedeAsync(IKey key)
        {
            var pk = key.ToBsonReferenceKey();
            var query = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq(Field.REFERENCE, pk),
                Builders<BsonDocument>.Filter.Eq(Field.STATE, Value.CURRENT));

            var update = Builders<BsonDocument>.Update.Set(Field.STATE, Value.SUPERCEDED);
            // A single delete on a sharded collection must contain an exact match on _id (and have the collection default collation) or contain the shard key (and have the simple collation).
            await _collection.UpdateManyAsync(query, update).ConfigureAwait(false);
        }
    }
}