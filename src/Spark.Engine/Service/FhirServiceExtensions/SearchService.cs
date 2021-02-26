namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Interfaces;
    using Spark.Engine.Core;
    using Spark.Engine.Extensions;
    using Task = System.Threading.Tasks.Task;

    public class SearchService : ISearchService, IServiceListener
    {
        private readonly IFhirModel _fhirModel;
        private readonly ILocalhost _localhost;
        private readonly IIndexService _indexService;
        private readonly IFhirIndex _fhirIndex;

        public SearchService(ILocalhost localhost, IFhirModel fhirModel, IFhirIndex fhirIndex, IIndexService indexService)
        {
            this._fhirModel = fhirModel;
            this._localhost = localhost;
            this._indexService = indexService;
            this._fhirIndex = fhirIndex;
        }

        public async Task<Snapshot> GetSnapshot(string type, SearchParams searchCommand)
        {
            Validate.TypeName(type);
            var results = await _fhirIndex.Search(type, searchCommand).ConfigureAwait(false);

            if (results.HasErrors)
            {
                throw new SparkException(HttpStatusCode.BadRequest, results.Outcome);
            }

            var builder = new UriBuilder(_localhost.Uri(type))
            {
                Query = results.UsedParameters
            };
            var link = builder.Uri;

            return CreateSnapshot(link, results, searchCommand);
        }

        public async Task<Snapshot> GetSnapshotForEverything(IKey key)
        {
            var searchCommand = new SearchParams();
            if (string.IsNullOrEmpty(key.ResourceId) == false)
            {
                searchCommand.Add("_id", key.ResourceId);
            }
            var compartment = _fhirModel.FindCompartmentInfo(key.TypeName);
            if (compartment != null)
            {
                foreach (var ri in compartment.ReverseIncludes)
                {
                    searchCommand.RevInclude.Add((ri, IncludeModifier.None));
                }
            }

            return await GetSnapshot(key.TypeName, searchCommand).ConfigureAwait(false);
        }

        private Snapshot CreateSnapshot(Uri selflink, IList<string> keys, SearchParams searchCommand)
        {
            var sort = GetFirstSort(searchCommand);

            var count = searchCommand.Count;
            if (count.HasValue)
            {
                //TODO: should we change count?
                //count = Math.Min(searchCommand.Count.Value, MAX_PAGE_SIZE);
                selflink = selflink.AddParam(SearchParams.SEARCH_PARAM_COUNT, new[] { count.ToString() });
            }

            if (searchCommand.Sort.Any())
            {
                foreach (var (item1, sortOrder) in searchCommand.Sort)
                {
                    selflink = selflink.AddParam(SearchParams.SEARCH_PARAM_SORT,
                        $"{item1}:{(sortOrder == SortOrder.Ascending ? "asc" : "desc")}");
                }
            }

            if (searchCommand.Include.Any())
            {
                selflink = selflink.AddParam(SearchParams.SEARCH_PARAM_INCLUDE, searchCommand.Include.Select(inc => inc.Item1).ToArray());
            }

            if (searchCommand.RevInclude.Any())
            {
                selflink = selflink.AddParam(SearchParams.SEARCH_PARAM_REVINCLUDE, searchCommand.RevInclude.Select(inc => inc.Item1).ToArray());
            }

            return Snapshot.Create(Bundle.BundleType.Searchset, selflink, keys, sort, count, searchCommand.Include.Select(inc => inc.Item1).ToList(),
                searchCommand.RevInclude.Select(inc => inc.Item1).ToList());
        }

        private static string GetFirstSort(SearchParams searchCommand)
        {
            string firstSort = null;
            if (searchCommand.Sort != null && searchCommand.Sort.Any())
            {
                firstSort = searchCommand.Sort[0].Item1; //TODO: Support sortorder and multiple sort arguments.
            }
            return firstSort;
        }

        public async Task<IKey> FindSingle(string type, SearchParams searchCommand)
        {
            return Key.ParseOperationPath((await GetSearchResults(type, searchCommand).ConfigureAwait(false)).Single());
        }

        public async Task<IKey> FindSingleOrDefault(string type, SearchParams searchCommand)
        {
            var value = (await GetSearchResults(type, searchCommand).ConfigureAwait(false)).SingleOrDefault();
            return value != null ? Key.ParseOperationPath(value) : null;
        }

        public async Task<SearchResults> GetSearchResults(string type, SearchParams searchCommand)
        {
            Validate.TypeName(type);
            var results = await _fhirIndex.Search(type, searchCommand).ConfigureAwait(false);

            return results.HasErrors ? throw new SparkException(HttpStatusCode.BadRequest, results.Outcome) : results;
        }

        public Task Inform(Uri location, Entry interaction)
        {
            return _indexService.Process(interaction);
        }
    }
}