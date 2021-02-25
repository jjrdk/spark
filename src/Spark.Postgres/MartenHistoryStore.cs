﻿namespace Spark.Postgres
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

    public class MartenHistoryStore : IHistoryStore
    {
        private readonly Func<IDocumentSession> _sessionFunc;

        public MartenHistoryStore(Func<IDocumentSession> sessionFunc)
        {
            _sessionFunc = sessionFunc;
        }

        /// <inheritdoc />
        public async Task<Snapshot> History(string typename, HistoryParameters parameters)
        {
            using var session = _sessionFunc();
            var query = session.Query<EntryEnvelope>().Where(e => e.ResourceType == typename);
            if (parameters.Since.HasValue)
            {
                query = query.Where(x => x.When > parameters.Since.Value);
            }

            // TODO: Handle sort

            if (parameters.Count.HasValue)
            {
                query = query.Take(parameters.Count.Value);
            }

            var result = await query.Select(x => new { x.Key.TypeName, x.Key.Base, x.Key.ResourceId, x.Key.VersionId })
                .ToListAsync()
                .ConfigureAwait(false);
            var keys = result.Select(x => new Key(x.Base, x.TypeName, x.ResourceId, x.VersionId).ToString()).ToList();
            return CreateSnapshot(keys, result.Count);
        }

        /// <inheritdoc />
        public async Task<Snapshot> History(IKey key, HistoryParameters parameters)
        {
            var storageKey = key.ToStorageKey();
            using var session = _sessionFunc();
            var query = session.Query<EntryEnvelope>().Where(e => e.ResourceKey == storageKey);
            if (parameters.Since.HasValue)
            {
                query = query.Where(x => x.When > parameters.Since.Value);
            }

            // TODO: Handle sort

            if (parameters.Count.HasValue)
            {
                query = query.Take(parameters.Count.Value);
            }

            var result = await query.Select(x => new { x.Key.TypeName, x.Key.Base, x.Key.ResourceId, x.Key.VersionId })
                .ToListAsync()
                .ConfigureAwait(false);
            return CreateSnapshot(
                result.Select(x => new Key(x.Base, x.TypeName, x.ResourceId, x.VersionId).ToString()).ToList(),
                result.Count);
        }

        /// <inheritdoc />
        public async Task<Snapshot> History(HistoryParameters parameters)
        {
            using var session = _sessionFunc();
            IQueryable<EntryEnvelope> query = session.Query<EntryEnvelope>();
            if (parameters.Since.HasValue)
            {
                query = query.Where(x => x.When > parameters.Since.Value);
            }

            // TODO: Handle sort

            if (parameters.Count.HasValue)
            {
                query = query.Take(parameters.Count.Value);
            }

            var result = await query.Select(x => new { x.Key.TypeName, x.Key.Base, x.Key.ResourceId, x.Key.VersionId })
                .ToListAsync()
                .ConfigureAwait(false);
            return CreateSnapshot(
                result.Select(x => new Key(x.Base, x.TypeName, x.ResourceId, x.VersionId).ToString()).ToList(),
                result.Count);
        }

        private static Snapshot CreateSnapshot(
            IList<string> keys,
            int? count = null,
            IList<string> includes = null,
            IList<string> reverseIncludes = null)
        {
            return Snapshot.Create(
                Bundle.BundleType.History,
                new Uri(RestOperation.HISTORY, UriKind.Relative),
                keys,
                "history",
                count,
                includes,
                reverseIncludes);
        }
    }
}
