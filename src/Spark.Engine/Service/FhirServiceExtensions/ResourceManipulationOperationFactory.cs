using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Spark.Engine.Core;
using Spark.Engine.Extensions;

namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System.Threading.Tasks;

    public static partial class ResourceManipulationOperationFactory
    {
        private static readonly Dictionary<Bundle.HTTPVerb, Func<Resource, IKey, ISearchService, SearchParams, Task<ResourceManipulationOperation>>> builders;
        private static ISearchService _searchService;

        static ResourceManipulationOperationFactory()
        {
            builders = new Dictionary<Bundle.HTTPVerb, Func<Resource, IKey, ISearchService, SearchParams, Task<ResourceManipulationOperation>>>();
            builders.Add(Bundle.HTTPVerb.POST, CreatePost);
            builders.Add(Bundle.HTTPVerb.PUT, CreatePut);
            builders.Add(Bundle.HTTPVerb.DELETE, CreateDelete);
        }

        public static async Task<ResourceManipulationOperation> CreatePost(Resource resource, IKey key, ISearchService service = null, SearchParams command = null)
        {
            _searchService = service;
            var searchResults = await GetSearchResult(key, command).ConfigureAwait(false);
            return new PostManipulationOperation(resource, key, searchResults, command);
        }

        private static Task<SearchResults> GetSearchResult(IKey key, SearchParams command = null)
        {
            if (command == null || command.Parameters.Count == 0)
                return null;
            if (command != null && _searchService == null)
                throw new InvalidOperationException("Unallowed operation");
            return _searchService.GetSearchResults(key.TypeName, command);
        }

        public static async Task<ResourceManipulationOperation> CreatePut(Resource resource, IKey key, ISearchService service = null, SearchParams command = null)
        {
            _searchService = service;
            var searchResults = await GetSearchResult(key, command).ConfigureAwait(false);
            return new PutManipulationOperation(resource, key, searchResults, command);
        }

        public static async Task<ResourceManipulationOperation> CreateDelete(IKey key, ISearchService service = null, SearchParams command = null)
        {
            _searchService = service;
            var searchResults = await GetSearchResult(key, command).ConfigureAwait(false);
            return new DeleteManipulationOperation(null, key, searchResults, command);
        }

        private static async Task<ResourceManipulationOperation> CreateDelete(Resource resource, IKey key, ISearchService service = null, SearchParams command = null)
        {
            _searchService = service;
            var searchResults = await GetSearchResult(key, command).ConfigureAwait(false);
            return new DeleteManipulationOperation(null, key, searchResults, command);
        }

        public static Task<ResourceManipulationOperation> GetManipulationOperation(Bundle.EntryComponent entryComponent, ILocalhost localhost, ISearchService service = null)
        {
            _searchService = service;
            var method = localhost.ExtrapolateMethod(entryComponent, null); //CCR: is key needed? Isn't method required?
            var key = localhost.ExtractKey(entryComponent);
            var searchUri = GetSearchUri(entryComponent, method);

            return builders[method](entryComponent.Resource, key, service, searchUri != null ? ParseQueryString(localhost, searchUri) : null);
        }

        private static Uri GetSearchUri(Bundle.EntryComponent entryComponent, Bundle.HTTPVerb method)
        {
            Uri searchUri = null;
            if (method == Bundle.HTTPVerb.POST)
            {
                searchUri = PostManipulationOperation.ReadSearchUri(entryComponent);
            }
            else if (method == Bundle.HTTPVerb.PUT)
            {
                searchUri = PutManipulationOperation.ReadSearchUri(entryComponent);
            }
            else if (method == Bundle.HTTPVerb.DELETE)
            {
                searchUri = DeleteManipulationOperation.ReadSearchUri(entryComponent);
            }
            return searchUri;
        }

        public static SearchParams ParseQueryString(ILocalhost localhost, Uri searchUri)
        {
            var keysCollection = searchUri.SplitParams();

            return SearchParams.FromUriParamList(keysCollection);
        }

    }
}