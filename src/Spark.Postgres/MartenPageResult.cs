// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Postgres
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Engine.Core;
    using Engine.Store.Interfaces;
    using Marten;

    internal class MartenPageResult<T> : IPageResult<T>
    {
        private readonly int _pageSize;

        private readonly Func<IDocumentSession> _session;
        private readonly Func<Entry, T> _transformFunc;

        public MartenPageResult(
            Func<IDocumentSession> session,
            int pageSize,
            long totalRecords,
            Func<Entry, T> transformFunc)
        {
            _session = session;
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

            using var session = _session();
            for (var offset = 0; offset < TotalRecords; offset += _pageSize)
            {
                var data = await session.Query<EntryEnvelope>()
                    .OrderBy(x => x.Id)
                    .Skip(offset)
                    .Take(_pageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);

                await callback(
                        data.Select(d => Entry.Create(d.Method, d.Key, d.Resource)).Select(_transformFunc).ToList())
                    .ConfigureAwait(false);
            }
        }
    }
}