﻿// /*
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
    using Engine.Store.Interfaces;
    using MongoDB.Bson;
    using MongoDB.Driver;

    internal class MongoCollectionPageResult<T> : IPageResult<T>
    {
        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly FilterDefinition<BsonDocument> _filter;
        private readonly int _pageSize;
        private readonly Func<BsonDocument, T> _transformFunc;

        public MongoCollectionPageResult(
            IMongoCollection<BsonDocument> collection,
            FilterDefinition<BsonDocument> filter,
            int pageSize,
            long totalRecords,
            Func<BsonDocument, T> transformFunc)
        {
            _collection = collection;
            _filter = filter;
            _pageSize = pageSize;
            _transformFunc = transformFunc;
            TotalRecords = totalRecords;
        }

        public long TotalRecords { get; }

        public long TotalPages => (long) Math.Ceiling(TotalRecords / (double) _pageSize);

        public async Task IterateAllPagesAsync(Func<IReadOnlyList<T>, Task> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            for (var offset = 0; offset < TotalRecords; offset += _pageSize)
            {
                var data = await _collection.Find(_filter)
                    .Sort(Builders<BsonDocument>.Sort.Ascending(Field.PRIMARYKEY))
                    .Skip(offset)
                    .Limit(_pageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);

                await callback(data.Select(d => _transformFunc(d)).ToList()).ConfigureAwait(false);
            }
        }
    }
}