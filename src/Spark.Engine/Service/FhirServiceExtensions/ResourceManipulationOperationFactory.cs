namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Spark.Engine.Core;
    using Spark.Engine.Extensions;
    using Task = System.Threading.Tasks.Task;

    public static partial class ResourceManipulationOperationFactory
    {
        private static readonly Dictionary<Bundle.HTTPVerb, Func<Resource, IKey, ISearchService, SearchParams, Task<ResourceManipulationOperation>>> builders;
        private static ISearchService searchService;

        static ResourceManipulationOperationFactory()
        {
            builders = new Dictionary<Bundle.HTTPVerb, Func<Resource, IKey, ISearchService, SearchParams, Task<ResourceManipulationOperation>>>();
            builders.Add(Bundle.HTTPVerb.POST, CreatePost);
            builders.Add(Bundle.HTTPVerb.PUT, CreatePut);
            builders.Add(Bundle.HTTPVerb.DELETE, CreateDelete);
        }

        public static async Task<ResourceManipulationOperation> CreatePost(Resource resource, IKey key, ISearchService service = null, SearchParams command = null)
        {
            searchService = service;
            return new PostManipulationOperation(resource, key, await GetSearchResultAsync(key, command).ConfigureAwait(false), command);
        }

        private static async Task<SearchResults> GetSearchResultAsync(IKey key, SearchParams command = null)
        {
            if (command == null || command.Parameters.Count == 0)
                return null;
            if (command != null && searchService == null)
                throw new InvalidOperationException("Unallowed operation");
            return await searchService.GetSearchResults(key.TypeName, command).ConfigureAwait(false);
        }

        public static async Task<ResourceManipulationOperation> CreatePut(Resource resource, IKey key, ISearchService service = null, SearchParams command = null)
        {
            searchService = service;
            return new PutManipulationOperation(resource, key, await GetSearchResultAsync(key, command).ConfigureAwait(false), command);
        }

        public static async Task<ResourceManipulationOperation> CreateDelete(IKey key, ISearchService service = null, SearchParams command = null)
        {
            searchService = service;
            return new DeleteManipulationOperation(null, key, await GetSearchResultAsync(key, command).ConfigureAwait(false), command);
        }

        private static async Task<ResourceManipulationOperation> CreateDelete(Resource resource, IKey key, ISearchService service = null, SearchParams command = null)
        {
            searchService = service;
            return new DeleteManipulationOperation(null, key, await GetSearchResultAsync(key, command).ConfigureAwait(false), command);
        }

        public static async Task<ResourceManipulationOperation> GetManipulationOperation(Bundle.EntryComponent entryComponent, ILocalhost localhost, ISearchService service = null)
        {
            searchService = service;
            var method = localhost.ExtrapolateMethod(entryComponent, null); //CCR: is key needed? Isn't method required?
            var key = localhost.ExtractKey(entryComponent);
            var searchUri = GetSearchUri(entryComponent, method);

            return await builders[method](entryComponent.Resource, key, service, searchUri != null ? ParseQueryString(localhost, searchUri) : null)
                .ConfigureAwait(false);
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

        private static SearchParams ParseQueryString(ILocalhost localhost, Uri searchUri)
        {

            var absoluteUri = localhost.Absolute(searchUri);
            var keysCollection = ParseQueryString(absoluteUri);

            return SearchParams.FromUriParamList(keysCollection);
        }

        private static IEnumerable<Tuple<string, string>> ParseQueryString(Uri uri)
        {
            var query = uri?.Query ?? throw new ArgumentNullException(nameof(uri));
            var collection = new NameValueCollection();
            return query.Trim('?')
                .Split('&')
                .Select(x => x.Split('='))
                .Where(x => x.Length == 2)
                .Select(x => Tuple.Create(Uri.UnescapeDataString(x[0]), Uri.UnescapeDataString(x[1])));
        }

    }
}