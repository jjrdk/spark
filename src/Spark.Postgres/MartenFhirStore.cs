namespace Spark.Postgres
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Engine.Core;
    using Engine.Extensions;
    using Engine.Store.Interfaces;
    using Marten;

    public class MartenFhirStore : IFhirStore
    {
        private readonly Func<IDocumentSession> _sessionFunc;

        public MartenFhirStore(Func<IDocumentSession> sessionFunc)
        {
            _sessionFunc = sessionFunc;
        }

        /// <inheritdoc />
        public async Task Add(Entry entry)
        {
            using var session = _sessionFunc();
            session.Store(new EntryEnvelope
            {
                Id = entry.Key.ToStorageKey(),
                ResourceType = entry.Resource.TypeName,
                State = entry.State,
                Key = entry.Key,
                Method = entry.Method,
                When = entry.When,
                Resource = entry.Resource
            });
            await session.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Entry> Get(IKey key)
        {
            using var session = _sessionFunc();
            var result = await session.LoadAsync<EntryEnvelope>(key.ToStorageKey()).ConfigureAwait(false);
            return Entry.Create(result.Method, result.Key, result.Resource);
        }

        /// <inheritdoc />
        public async Task<IList<Entry>> Get(IEnumerable<IKey> localIdentifiers)
        {
            var localKeys = localIdentifiers.Select(x => x.ToStorageKey()).ToArray();
            using var session = _sessionFunc();
            var results = await session.Query<EntryEnvelope>().Where(e => e.Id.IsOneOf(localKeys)).ToListAsync().ConfigureAwait(false);
            return results.Select(result => Entry.Create(result.Method, result.Key, result.Resource)).ToList();
        }
    }
}
