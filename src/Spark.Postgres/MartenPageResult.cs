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
        public long TotalRecords { get; }

        public long TotalPages => (long)Math.Ceiling(TotalRecords / (double)_pageSize);

        private readonly Func<IDocumentSession> _session;
        private readonly int _pageSize;
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