namespace Spark.Postgres
{
    using System;
    using System.Threading.Tasks;
    using Engine.Core;
    using Engine.Store.Interfaces;
    using Marten;
    using Microsoft.Extensions.Logging;

    public class MartenSnapshotStore : ISnapshotStore
    {
        private readonly Func<IDocumentSession> _sessionFunc;
        private readonly ILogger<MartenSnapshotStore> _logger;

        public MartenSnapshotStore(Func<IDocumentSession> sessionFunc, ILogger<MartenSnapshotStore> logger)
        {
            _sessionFunc = sessionFunc;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task AddSnapshot(Snapshot snapshot)
        {
            _logger.LogDebug("Snapshot added");
            using var session = _sessionFunc();
            session.Store(snapshot);
            await session.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Snapshot> GetSnapshot(string snapshotid)
        {
            _logger.LogDebug("Returned snapshot " + snapshotid);
            using var session = _sessionFunc();
            var snapshot = await session.LoadAsync<Snapshot>(snapshotid).ConfigureAwait(false);

            return snapshot;
        }
    }
}