namespace Spark.Postgres
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Engine.Auxiliary;
    using Engine.Core;
    using Engine.Extensions;
    using Engine.Store.Interfaces;
    using Hl7.Fhir.Model;
    using Marten;
    using Expression = System.Linq.Expressions.Expression;

    public class MartenHistoryStore : IHistoryStore
    {
        private readonly Func<IDocumentSession> _sessionFunc;

        public MartenHistoryStore(Func<IDocumentSession> sessionFunc)
        {
            _sessionFunc = sessionFunc;
        }

        public async Task<Snapshot> History(string resource, HistoryParameters parameters)
        {
            using var session = _sessionFunc();
            var entryEnvelopes = session.Query<EntryEnvelope>()
                .Where(e => e.ResourceType == resource);
            if (parameters.Since.HasValue)
            {
                entryEnvelopes = entryEnvelopes.Where(e => e.When > parameters.Since.Value);
            }
            if (parameters.Count.HasValue)
            {
                entryEnvelopes = entryEnvelopes.Take(parameters.Count.Value);
            }

            if (parameters.SortBy != null)
            {
                var parameter = Expression.Parameter(typeof(EntryEnvelope), "x");
                entryEnvelopes = entryEnvelopes.OrderBy(
                    Expression.Lambda<Func<EntryEnvelope, object>>(
                        Expression.Property(parameter, parameters.SortBy),
                        parameter));
            }
            var keys = await entryEnvelopes
                .Select(e => e.Key)
                .ToListAsync()
                .ConfigureAwait(false);
            var primaryKeys = keys.Select(k => k.ToStorageKey()).ToArray();
            return CreateSnapshot(primaryKeys, parameters.Count);
        }

        public async Task<Snapshot> History(IKey key, HistoryParameters parameters)
        {
            using var session = _sessionFunc();
            var entryEnvelopes = session.Query<EntryEnvelope>()
                .Where(e => e.Key.ResourceId == key.ResourceId && e.Key.TypeName == key.TypeName);
            if (parameters.Since.HasValue)
            {
                entryEnvelopes = entryEnvelopes.Where(e => e.When > parameters.Since.Value);
            }
            if (parameters.Count.HasValue)
            {
                entryEnvelopes = entryEnvelopes.Take(parameters.Count.Value);
            }

            if (parameters.SortBy != null)
            {
                var parameter = Expression.Parameter(typeof(EntryEnvelope), "x");
                entryEnvelopes = entryEnvelopes.OrderBy(
                    Expression.Lambda<Func<EntryEnvelope, object>>(
                        Expression.Property(parameter, parameters.SortBy),
                        parameter));
            }
            var keys = await entryEnvelopes
                .Select(e => e.Key)
                .ToListAsync()
                .ConfigureAwait(false);
            var primaryKeys = keys.Select(k => k.ToStorageKey()).ToArray();
            return CreateSnapshot(primaryKeys, parameters.Count);
        }

        public async Task<Snapshot> History(HistoryParameters parameters)
        {
            using var session = _sessionFunc();
            IQueryable<EntryEnvelope> entryEnvelopes = session.Query<EntryEnvelope>();
            if (parameters.Since.HasValue)
            {
                entryEnvelopes = entryEnvelopes.Where(e => e.When > parameters.Since.Value);
            }
            if (parameters.Count.HasValue)
            {
                entryEnvelopes = entryEnvelopes.Take(parameters.Count.Value);
            }

            if (parameters.SortBy != null)
            {
                var parameter = Expression.Parameter(typeof(EntryEnvelope), "x");
                entryEnvelopes = entryEnvelopes.OrderBy(
                    Expression.Lambda<Func<EntryEnvelope, object>>(
                        Expression.Property(parameter, parameters.SortBy),
                        parameter));
            }
            var keys = await entryEnvelopes
                .Select(e => e.Key)
                .ToListAsync()
                .ConfigureAwait(false);
            var primaryKeys = keys.Select(k => k.ToStorageKey()).ToArray();
            return CreateSnapshot(primaryKeys, parameters.Count);
        }

        private static Snapshot CreateSnapshot(string[] keys, int? count = null, IList<string> includes = null, IList<string> reverseIncludes = null)
        {
            var link = new Uri(RestOperation.HISTORY, UriKind.Relative);
            var snapshot = Snapshot.Create(Bundle.BundleType.History, link, keys, "history", count, includes, reverseIncludes);
            return snapshot;
        }
    }
}
