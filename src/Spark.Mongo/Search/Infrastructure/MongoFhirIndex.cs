/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

namespace Spark.Mongo.Search.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Engine;
    using Engine.Core;
    using Engine.Interfaces;
    using Engine.Search;
    using Engine.Store.Interfaces;
    using Hl7.Fhir.Rest;
    using Searcher;

    public class MongoFhirIndex : IFhirIndex
    {
        private readonly MongoSearcher _searcher;
        private readonly IIndexStore _indexStore;
        private readonly SearchSettings _searchSettings;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public MongoFhirIndex(IIndexStore indexStore, MongoSearcher searcher, SparkSettings sparkSettings = null)
        {
            _indexStore = indexStore;
            _searcher = searcher;
            _searchSettings = sparkSettings?.Search ?? new SearchSettings();
        }

        public async Task Clean()
        {
            try
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);
                await _indexStore.Clean().ConfigureAwait(false);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public Task<SearchResults> Search(string resource, SearchParams searchCommand)
        {
            return _searcher.Search(resource, searchCommand, _searchSettings);
        }

        public async Task<Key> FindSingle(string resource, SearchParams searchCommand)
        {
            // todo: this needs optimization

            var results = await _searcher.Search(resource, searchCommand, _searchSettings).ConfigureAwait(false);
            if (results.Count > 1)
            {
                throw Error.BadRequest("The search for a single resource yielded more than one.");
            }
            else if (results.Count == 0)
            {
                throw Error.BadRequest("No resources were found while searching for a single resource.");
            }
            else
            {
                var location = results.FirstOrDefault();
                return Key.ParseOperationPath(location);
            }
        }
        public Task<SearchResults> GetReverseIncludes(IList<IKey> keys, IList<string> revIncludes)
        {
            return _searcher.GetReverseIncludes(keys, revIncludes);
        }

    }
}
