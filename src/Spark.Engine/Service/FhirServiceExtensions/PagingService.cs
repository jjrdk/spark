﻿namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System;
    using System.Threading.Tasks;
    using Spark.Engine.Core;
    using Spark.Engine.Store.Interfaces;

    public class PagingService : IPagingService
    {
        private readonly ISnapshotStore _snapshotstore;
        private readonly ISnapshotPaginationProvider _paginationProvider;

        public PagingService(ISnapshotStore snapshotstore, ISnapshotPaginationProvider paginationProvider)
        {
            _snapshotstore = snapshotstore;
            _paginationProvider = paginationProvider;
        }

        public async Task<ISnapshotPagination> StartPagination(Snapshot snapshot)
        {
            if (_snapshotstore != null)
            {
                await _snapshotstore.AddSnapshot(snapshot).ConfigureAwait(false);
            }
            else
            {
                snapshot.Id = null;
            }

            return _paginationProvider.StartPagination(snapshot);
        }

        public async Task<ISnapshotPagination> StartPagination(string snapshotkey)
        {
            return _snapshotstore == null
                ? throw new NotSupportedException("Stateful pagination is not currently supported.")
                : _paginationProvider.StartPagination(
                await _snapshotstore.GetSnapshot(snapshotkey).ConfigureAwait(false));
        }
    }
}
