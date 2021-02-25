﻿/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

namespace Spark.Store.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Spark.Engine.Core;
    using Spark.Engine.Store.Interfaces;
    using Engine.Extensions;
    using Spark.Mongo.Search.Infrastructure;
    using Spark.Mongo.Store;

    public class MongoFhirStore : IFhirStore
    {
        readonly IMongoDatabase database;
        readonly IMongoCollection<BsonDocument> collection;

        public MongoFhirStore(string mongoUrl)
        {
            this.database = MongoDatabaseFactory.GetMongoDatabase(mongoUrl);
            this.collection = database.GetCollection<BsonDocument>(Collection.RESOURCE);
            //this.transaction = new MongoSimpleTransaction(collection);
        }

        public async Task Add(Entry entry)
        {
            var document = entry.ToBsonDocument();
            await SupercedeAsync(entry.Key).ConfigureAwait(false);
            await collection.InsertOneAsync(document).ConfigureAwait(false);
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

            var document = (await collection.FindAsync(query).ConfigureAwait(false)).FirstOrDefault();
            return document.ToEntry();

        }

        public async Task<IList<Entry>> Get(IEnumerable<IKey> identifiers)
        {
            if (!identifiers.Any())
                return new List<Entry>();

            IList<IKey> identifiersList = identifiers.ToList();
            var versionedIdentifiers = GetBsonValues(identifiersList, k => k.HasVersionId());
            var unversionedIdentifiers = GetBsonValues(identifiersList, k => k.HasVersionId() == false);

            var queries = new List<FilterDefinition<BsonDocument>>();
            if (versionedIdentifiers.Any())
                queries.Add(GetSpecificVersionQuery(versionedIdentifiers));
            if (unversionedIdentifiers.Any())
                queries.Add(GetCurrentVersionQuery(unversionedIdentifiers));
            var query = Builders<BsonDocument>.Filter.Or(queries);

            var cursor = (await collection.FindAsync(query).ConfigureAwait(false)).ToEnumerable();

            return cursor.ToEntries().ToList();
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
                Builders<BsonDocument>.Filter.Eq(Field.STATE, Value.CURRENT)
            );

            var update = Builders<BsonDocument>.Update.Set(Field.STATE, Value.SUPERCEDED);
            // A single delete on a sharded collection must contain an exact match on _id (and have the collection default collation) or contain the shard key (and have the simple collation).
            await collection.UpdateManyAsync(query, update).ConfigureAwait(false);
        }

    }
}
