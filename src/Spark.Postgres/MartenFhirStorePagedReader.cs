namespace Spark.Postgres
{
    using System;
    using System.Threading.Tasks;
    using Engine.Core;
    using Engine.Store.Interfaces;
    using Marten;

    public class MartenFhirStorePagedReader : IFhirStorePagedReader
    {
        private readonly Func<IDocumentSession> _sessionFunc;

        public MartenFhirStorePagedReader(Func<IDocumentSession> sessionFunc)
        {
            _sessionFunc = sessionFunc;
        }

        /// <inheritdoc />
        public async Task<IPageResult<Entry>> ReadAsync(FhirStorePageReaderOptions options = null)
        {
            var pagesize = options?.PageSize ?? 100;
            var total = 0;
            using (var session = _sessionFunc())
            {
                total = await session.Query<EntryEnvelope>().CountAsync().ConfigureAwait(false);
            }
            return new MartenPageResult<Entry>(_sessionFunc, pagesize, total, e => e);
        }
    }
}