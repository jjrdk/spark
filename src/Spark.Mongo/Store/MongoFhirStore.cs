/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

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
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<BsonDocument> collection;

        public MongoFhirStore(string mongoUrl)
        {
            this.database = MongoDatabaseFactory.GetMongoDatabase(mongoUrl);
            this.collection = database.GetCollection<BsonDocument>(Collection.RESOURCE);
        }

        public async Task Add(Entry entry)
        {
            var document = SparkBsonHelper.ToBsonDocument(entry);
            await Supercede(entry.Key).ConfigureAwait(false);
            await collection.InsertOneAsync(document).ConfigureAwait(false);
        }

        public async Task<Entry> Get(IKey key)
        {
            var clauses = new List<FilterDefinition<BsonDocument>>
            {
                Builders<BsonDocument>.Filter.Eq(Field.TYPENAME, key.TypeName),
                Builders<BsonDocument>.Filter.Eq(Field.RESOURCEID, key.ResourceId)
            };


            if (key.HasVersionId())
            {
                clauses.Add(Builders<BsonDocument>.Filter.Eq(Field.VERSIONID, key.VersionId));
            }
            else
            {
                clauses.Add(Builders<BsonDocument>.Filter.Eq(Field.STATE, Value.CURRENT));
            }

            var query = Builders<BsonDocument>.Filter.And(clauses);

            var result = await collection.FindAsync(query).ConfigureAwait(false);
            var document = result.FirstOrDefault();
            return document.ToEntry();

        }

        public async Task<IList<Entry>> Get(IEnumerable<IKey> identifiers)
        {
            if (!identifiers.Any())
            {
                return Array.Empty<Entry>();
            }

            IList<IKey> identifiersList = identifiers.ToList();
            var versionedIdentifiers = GetBsonValues(identifiersList, k => k.HasVersionId());
            var unversionedIdentifiers = GetBsonValues(identifiersList, k => k.HasVersionId() == false);

            var queries = new List<FilterDefinition<BsonDocument>>();
            if (versionedIdentifiers.Any())
                queries.Add(GetSpecificVersionQuery(versionedIdentifiers));
            if (unversionedIdentifiers.Any())
                queries.Add(GetCurrentVersionQuery(unversionedIdentifiers));
            var query = Builders<BsonDocument>.Filter.Or(queries);

            var asyncCursor = await collection.FindAsync(query).ConfigureAwait(false);
            var cursor = asyncCursor.ToEnumerable();

            return cursor.ToEntries().ToList();
        }

        private IEnumerable<BsonValue> GetBsonValues(IEnumerable<IKey> identifiers, Func<IKey, bool> keyCondition)
        {
            return identifiers.Where(keyCondition).Select(k => (BsonValue)k.ToString());
        }

        private FilterDefinition<BsonDocument> GetCurrentVersionQuery(IEnumerable<BsonValue> ids)
        {
            var clauses = new List<FilterDefinition<BsonDocument>>
            {
                Builders<BsonDocument>.Filter.In(Field.REFERENCE, ids),
                Builders<BsonDocument>.Filter.Eq(Field.STATE, Value.CURRENT)
            };
            return Builders<BsonDocument>.Filter.And(clauses);

        }

        private FilterDefinition<BsonDocument> GetSpecificVersionQuery(IEnumerable<BsonValue> ids)
        {
            var clauses =
                new List<FilterDefinition<BsonDocument>> {Builders<BsonDocument>.Filter.In(Field.PRIMARYKEY, ids)};

            return Builders<BsonDocument>.Filter.And(clauses);
        }

        private Task Supercede(IKey key)
        {
            var pk = key.ToBsonReferenceKey();
            var query = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq(Field.REFERENCE, pk),
                Builders<BsonDocument>.Filter.Eq(Field.STATE, Value.CURRENT)
            );

            var update = Builders<BsonDocument>.Update.Set(Field.STATE, Value.SUPERCEDED);
            // A single delete on a sharded collection must contain an exact match on _id (and have the collection default collation) or contain the shard key (and have the simple collation).
            return collection.UpdateManyAsync(query, update);
        }
    }
}
