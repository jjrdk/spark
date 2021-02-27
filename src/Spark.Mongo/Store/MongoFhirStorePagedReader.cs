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
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Search.Infrastructure;

    public class MongoFhirStorePagedReader : IFhirStorePagedReader
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public MongoFhirStorePagedReader(string mongoUrl)
        {
            var database = MongoDatabaseFactory.GetMongoDatabase(mongoUrl);
            _collection = database.GetCollection<BsonDocument>(Collection.RESOURCE);
        }

        public async Task<IPageResult<Entry>> ReadAsync(FhirStorePageReaderOptions options)
        {
            options ??= new FhirStorePageReaderOptions();

            var filter = Builders<BsonDocument>.Filter.Empty;

            var totalRecords = await _collection.CountDocumentsAsync(filter).ConfigureAwait(false);

            return new MongoCollectionPageResult<Entry>(
                _collection,
                filter,
                options.PageSize,
                totalRecords,
                document => document.ToEntry());
        }
    }
}