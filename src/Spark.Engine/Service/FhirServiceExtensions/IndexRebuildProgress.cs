// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System;
    using System.Threading.Tasks;

    internal class IndexRebuildProgress
    {
        private const int INDEX_CLEAR_PROGRESS_PERCENTAGE = 10;

        private readonly IIndexBuildProgressReporter _reporter;
        private int _overallProgress;
        private int _recordsProcessed;
        private int _remainingProgress = 100;

        public IndexRebuildProgress(IIndexBuildProgressReporter reporter) => _reporter = reporter;

        public async Task StartedAsync()
        {
            await ReportProgressAsync("Index rebuild started").ConfigureAwait(false);
        }

        public async Task CleanStartedAsync()
        {
            await ReportProgressAsync("Clearing index").ConfigureAwait(false);
        }

        public async Task CleanCompletedAsync()
        {
            _overallProgress += INDEX_CLEAR_PROGRESS_PERCENTAGE;
            await ReportProgressAsync("Index cleared").ConfigureAwait(false);
            _remainingProgress -= _overallProgress;
        }

        public async Task RecordsProcessedAsync(int records, long total)
        {
            _recordsProcessed += records;
            _overallProgress += (int) (_remainingProgress / (double) total * records);
            await ReportProgressAsync($"{_recordsProcessed} records processed").ConfigureAwait(false);
        }

        public async Task DoneAsync()
        {
            _overallProgress = 100;
            await ReportProgressAsync("Index rebuild done").ConfigureAwait(false);
        }

        public async Task ErrorAsync(string error)
        {
            if (_reporter == null)
            {
                return;
            }

            await _reporter.ReportErrorAsync(error).ConfigureAwait(false);
        }

        private async Task ReportProgressAsync(string message)
        {
            if (_reporter == null)
            {
                return;
            }

            await _reporter.ReportProgressAsync(_overallProgress, message).ConfigureAwait(false);
        }
    }
}
