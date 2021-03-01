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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using Extensions;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;

    public static partial class ResourceManipulationOperationFactory
    {
        private static readonly
            Dictionary<Bundle.HTTPVerb,
                Func<Resource, IKey, ISearchService, SearchParams, Task<ResourceManipulationOperation>>> _builders;

        static ResourceManipulationOperationFactory()
        {
            _builders =
                new Dictionary<Bundle.HTTPVerb, Func<Resource, IKey, ISearchService, SearchParams,
                    Task<ResourceManipulationOperation>>>
                {
                    {Bundle.HTTPVerb.POST, CreatePost},
                    {Bundle.HTTPVerb.PUT, CreatePut},
                    {Bundle.HTTPVerb.DELETE, CreateDelete}
                };
        }

        public static async Task<ResourceManipulationOperation> CreatePost(
            this Resource resource,
            IKey key,
            ISearchService service = null,
            SearchParams command = null) =>
            new PostManipulationOperation(
                resource,
                key,
                await GetSearchResultAsync(key, service, command).ConfigureAwait(false),
                command);

        private static async Task<SearchResults> GetSearchResultAsync(
            IKey key,
            ISearchService searchService = null,
            SearchParams command = null)
        {
            if (command == null || command.Parameters.Count == 0)
            {
                return null;
            }

            return searchService == null
                ? throw new InvalidOperationException("Invalid operation")
                : await searchService.GetSearchResults(key.TypeName, command).ConfigureAwait(false);
        }

        public static async Task<ResourceManipulationOperation> CreatePut(
            this Resource resource,
            IKey key,
            ISearchService service = null,
            SearchParams command = null) =>
            new PutManipulationOperation(
                resource,
                key,
                await GetSearchResultAsync(key, service, command).ConfigureAwait(false),
                command);

        public static async Task<ResourceManipulationOperation> CreateDelete(
            this IKey key,
            ISearchService service = null,
            SearchParams command = null) =>
            new DeleteManipulationOperation(
                null,
                key,
                await GetSearchResultAsync(key, service, command).ConfigureAwait(false),
                command);

        private static async Task<ResourceManipulationOperation> CreateDelete(
            Resource resource,
            IKey key,
            ISearchService service = null,
            SearchParams command = null) =>
            new DeleteManipulationOperation(
                null,
                key,
                await GetSearchResultAsync(key, service, command).ConfigureAwait(false),
                command);

        public static async Task<ResourceManipulationOperation> GetManipulationOperation(
            Bundle.EntryComponent entryComponent,
            ILocalhost localhost,
            ISearchService service = null)
        {
            var method = localhost.ExtrapolateMethod(entryComponent, null); //CCR: is key needed? Isn't method required?
            var key = localhost.ExtractKey(entryComponent);
            var searchUri = GetSearchUri(entryComponent, method);

            return await _builders[method](
                    entryComponent.Resource,
                    key,
                    service,
                    searchUri != null ? ParseQueryString(localhost, searchUri) : null)
                .ConfigureAwait(false);
        }

        private static Uri GetSearchUri(Bundle.EntryComponent entryComponent, Bundle.HTTPVerb method)
        {
            var searchUri = method switch
            {
                Bundle.HTTPVerb.POST => PostManipulationOperation.ReadSearchUri(entryComponent),
                Bundle.HTTPVerb.PUT => PutManipulationOperation.ReadSearchUri(entryComponent),
                Bundle.HTTPVerb.DELETE => DeleteManipulationOperation.ReadSearchUri(entryComponent),
                _ => null
            };
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
            return query.Trim('?')
                .Split('&')
                .Select(x => x.Split('='))
                .Where(x => x.Length == 2)
                .Select(x => Tuple.Create(Uri.UnescapeDataString(x[0]), Uri.UnescapeDataString(x[1])));
        }
    }
}