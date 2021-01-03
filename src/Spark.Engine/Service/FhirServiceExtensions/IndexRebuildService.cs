using Spark.Engine.Core;
using Spark.Engine.Maintenance;
using Spark.Engine.Store.Interfaces;
using System;
using System.Threading.Tasks;

namespace Spark.Engine.Service.FhirServiceExtensions
{
    public class IndexRebuildService : IIndexRebuildService
    {
        private readonly IIndexStore _indexStore;
        private readonly IIndexService _indexService;
        private readonly IFhirStorePagedReader _entryReader;
        private readonly SparkSettings _sparkSettings;

        public IndexRebuildService(
            IIndexStore indexStore,
            IIndexService indexService,
            IFhirStorePagedReader entryReader,
            SparkSettings sparkSettings)
        {
            _indexStore = indexStore ?? throw new ArgumentNullException(nameof(indexStore));
            _indexService = indexService ?? throw new ArgumentNullException(nameof(indexService));
            _entryReader = entryReader ?? throw new ArgumentNullException(nameof(entryReader));
            _sparkSettings = sparkSettings ?? throw new ArgumentNullException(nameof(sparkSettings));
        }

        public async Task RebuildIndexAsync(IIndexBuildProgressReporter reporter = null)
        {
            using (MaintenanceMode.Enable(MaintenanceLockMode.Write)) // allow to read data while reindexing
            {
                var progress = new IndexRebuildProgress(reporter);
                await progress.StartedAsync().ConfigureAwait(false);

                // TODO: lock collections for writing somehow?

                var indexSettings = _sparkSettings.IndexSettings ?? new IndexSettings();
                if (indexSettings.ClearIndexOnRebuild)
                {
                    await progress.CleanStartedAsync().ConfigureAwait(false);
                    await _indexStore.Clean().ConfigureAwait(false);
                    await progress.CleanCompletedAsync().ConfigureAwait(false);
                }

                var paging = await _entryReader.ReadAsync(new FhirStorePageReaderOptions
                {
                    PageSize = indexSettings.ReindexBatchSize
                }).ConfigureAwait(false);

                await paging.IterateAllPagesAsync(async entries =>
                {
                    // Selecting records page-by-page (page size is defined in app config, default is 100).
                    // This will help to keep memory usage under control.
                    foreach (var entry in entries)
                    {
                        // TODO: use BulkWrite operation for this
                        // TODO: use async API
                        try
                        {
                            await _indexService.Process(entry).ConfigureAwait(false);
                        }
                        catch (Exception)
                        {
                            // TODO: log exception!
                            await progress.ErrorAsync($"Failed to reindex entry {entry.Key}").ConfigureAwait(false);
                        }
                    }

                    await progress.RecordsProcessedAsync(entries.Count, paging.TotalRecords)
                        .ConfigureAwait(false);

                }).ConfigureAwait(false);

                // TODO: - unlock collections for writing

                await progress.DoneAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}
