// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Mongo.Store.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Engine.Auxiliary;
    using Engine.Core;
    using Engine.Store.Interfaces;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Search.Infrastructure;

    public class HistoryStore : IHistoryStore
    {
        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly IMongoDatabase _database;

        public HistoryStore(string mongoUrl)
        {
            _database = MongoDatabaseFactory.GetMongoDatabase(mongoUrl);
            _collection = _database.GetCollection<BsonDocument>(Collection.RESOURCE);
        }

        public async Task<Snapshot> History(string resource, HistoryParameters parameters)
        {
            var clauses =
                new List<FilterDefinition<BsonDocument>> {Builders<BsonDocument>.Filter.Eq(Field.TYPENAME, resource)};

            if (parameters.Since != null)
            {
                clauses.Add(Builders<BsonDocument>.Filter.Gt(Field.WHEN, BsonDateTime.Create(parameters.Since)));
            }

            var primaryKeys = await FetchPrimaryKeys(clauses).ConfigureAwait(false);
            return CreateSnapshot(primaryKeys, parameters.Count);
        }

        public async Task<Snapshot> History(IKey key, HistoryParameters parameters)
        {
            var clauses = new List<FilterDefinition<BsonDocument>>
            {
                Builders<BsonDocument>.Filter.Eq(Field.TYPENAME, key.TypeName),
                Builders<BsonDocument>.Filter.Eq(Field.RESOURCEID, key.ResourceId)
            };

            if (parameters.Since != null)
            {
                clauses.Add(Builders<BsonDocument>.Filter.Gt(Field.WHEN, BsonDateTime.Create(parameters.Since)));
            }

            var primaryKeys = await FetchPrimaryKeys(clauses).ConfigureAwait(false);
            return CreateSnapshot(primaryKeys, parameters.Count);
        }

        public async Task<Snapshot> History(HistoryParameters parameters)
        {
            var clauses = new List<FilterDefinition<BsonDocument>>();
            if (parameters.Since != null)
            {
                clauses.Add(Builders<BsonDocument>.Filter.Gt(Field.WHEN, BsonDateTime.Create(parameters.Since)));
            }

            var primaryKeys = await FetchPrimaryKeys(clauses).ConfigureAwait(false);
            return CreateSnapshot(primaryKeys, parameters.Count);
        }

        public async Task<IList<string>> FetchPrimaryKeys(FilterDefinition<BsonDocument> query)
        {
            var result = await _collection.FindAsync(
                    query,
                    new FindOptions<BsonDocument>
                    {
                        Sort = Builders<BsonDocument>.Sort.Descending(Field.WHEN),
                        Projection = Builders<BsonDocument>.Projection.Include(Field.PRIMARYKEY)
                    })
                .ConfigureAwait(false);
            return result.ToEnumerable().Select(doc => doc.GetValue(Field.PRIMARYKEY).AsString).ToList();
        }

        private Task<IList<string>> FetchPrimaryKeys(IEnumerable<FilterDefinition<BsonDocument>> clauses)
        {
            var query = clauses.Any()
                ? Builders<BsonDocument>.Filter.And(clauses)
                : Builders<BsonDocument>.Filter.Empty;
            return FetchPrimaryKeys(query);
        }

        private static Snapshot CreateSnapshot(
            IList<string> keys,
            int? count = null,
            IList<string> includes = null,
            IList<string> reverseIncludes = null)
        {
            var link = new Uri(TransactionBuilder.HISTORY, UriKind.Relative);
            var snapshot = Snapshot.Create(
                Bundle.BundleType.History,
                link,
                keys,
                "history",
                count,
                includes,
                reverseIncludes);
            return snapshot;
        }
    }
}