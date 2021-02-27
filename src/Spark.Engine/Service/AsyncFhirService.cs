// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Service
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Core;
    using Extensions;
    using FhirResponseFactory;
    using FhirServiceExtensions;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Store;
    using Task = System.Threading.Tasks.Task;

    public class AsyncFhirService : ExtendableWith<IFhirServiceExtension>, IAsyncFhirService
    {
        // CCR: FhirService now implements InteractionHandler that is used by the TransactionService to actually perform the operation.
        // This creates a circular reference that is solved by sending the handler on each call.
        // A future step might be to split that part into a different service (maybe StorageService?)

        private readonly IInteractionHandler _interactionHandler;
        private readonly IFhirResponseFactory _responseFactory;
        private readonly ICompositeServiceListener _serviceListener;

        public AsyncFhirService(
            IInteractionHandler interactionHandler,
            IFhirServiceExtension[] extensions,
            IFhirResponseFactory responseFactory, // TODO: can we remove this dependency?
            ICompositeServiceListener serviceListener = null) // TODO: can we remove this dependency? - CCR
        {
            _interactionHandler = interactionHandler;
            _responseFactory = responseFactory ?? throw new ArgumentNullException(nameof(responseFactory));
            _serviceListener = serviceListener ?? throw new ArgumentNullException(nameof(serviceListener));

            foreach (var serviceExtension in extensions)
            {
                AddExtension(serviceExtension);
            }
        }

        public async Task<FhirResponse> AddMeta(IKey key, Parameters parameters)
        {
            var storageService = GetFeature<IResourceStorageService>();
            var entry = await storageService.Get(key).ConfigureAwait(false);
            if (entry != null && !entry.IsDeleted())
            {
                entry.Resource.AffixTags(parameters);
                await storageService.Add(entry).ConfigureAwait(false);
            }

            return _responseFactory.GetMetadataResponse(entry, key);
        }

        public Task<FhirResponse> ConditionalCreate(
            IKey key,
            Resource resource,
            IEnumerable<Tuple<string, string>> parameters) =>
            ConditionalCreate(key, resource, SearchParams.FromUriParamList(parameters));

        public async Task<FhirResponse> ConditionalCreate(IKey key, Resource resource, SearchParams parameters)
        {
            var searchStore = GetFeature<ISearchService>();
            var transactionService = GetFeature<ITransactionService>();
            var operation = await resource.CreatePost(key, searchStore, parameters).ConfigureAwait(false);
            return await transactionService.HandleTransaction(operation, _interactionHandler).ConfigureAwait(false);
        }

        public async Task<FhirResponse> ConditionalDelete(IKey key, IEnumerable<Tuple<string, string>> parameters)
        {
            var searchStore = GetFeature<ISearchService>();
            var transactionService = GetFeature<ITransactionService>();
            var deleteOperation = await key.CreateDelete(searchStore, SearchParams.FromUriParamList(parameters))
                .ConfigureAwait(false);
            return await transactionService.HandleTransaction(deleteOperation, _interactionHandler).ConfigureAwait(false)
                   ?? Respond.WithCode(HttpStatusCode.NotFound);
        }

        public async Task<FhirResponse> ConditionalUpdate(IKey key, Resource resource, SearchParams parameters)
        {
            var searchStore = GetFeature<ISearchService>();
            var transactionService = GetFeature<ITransactionService>();

            // FIXME: if update receives a key with no version how do we handle concurrency?

            var operation = await resource.CreatePut(key, searchStore, parameters).ConfigureAwait(false);
            return await transactionService.HandleTransaction(operation, _interactionHandler).ConfigureAwait(false);
        }

        public Task<FhirResponse> CapabilityStatement(string sparkVersion)
        {
            var capabilityStatementService = GetFeature<ICapabilityStatementService>();
            var response = Respond.WithResource(capabilityStatementService.GetSparkCapabilityStatement(sparkVersion));
            return Task.FromResult(response);
        }

        public async Task<FhirResponse> Create(IKey key, Resource resource)
        {
            Validate.Key(key);
            Validate.HasTypeName(key);
            Validate.ResourceType(key, resource);

            key = key.CleanupForCreate();
            var result = await Store(Entry.Post(key, resource)).ConfigureAwait(false);
            return Respond.WithResource(HttpStatusCode.Created, result);
        }

        public async Task<FhirResponse> Delete(IKey key)
        {
            Validate.Key(key);
            Validate.HasNoVersion(key);

            var resourceStorage = GetFeature<IResourceStorageService>();

            var current = await resourceStorage.Get(key).ConfigureAwait(false);
            return current != null && current.IsPresent
                ? await Delete(Entry.Delete(key, DateTimeOffset.UtcNow)).ConfigureAwait(false)
                : Respond.WithCode(HttpStatusCode.NotFound);
        }

        public async Task<FhirResponse> Delete(Entry entry)
        {
            Validate.Key(entry.Key);
            await Store(entry).ConfigureAwait(false);
            return Respond.WithCode(HttpStatusCode.NoContent);
        }

        public async Task<FhirResponse> GetPage(string snapshotKey, int index)
        {
            var pagingExtension = GetFeature<IPagingService>();
            var snapshot = await pagingExtension.StartPagination(snapshotKey).ConfigureAwait(false);
            var page = await snapshot.GetPage(index).ConfigureAwait(false);
            return _responseFactory.GetFhirResponse(page);
        }

        public async Task<FhirResponse> History(HistoryParameters parameters)
        {
            var historyExtension = GetFeature<IHistoryService>();
            var snapshot = await historyExtension.History(parameters).ConfigureAwait(false);
            return await CreateSnapshotResponse(snapshot).ConfigureAwait(false);
        }

        public async Task<FhirResponse> History(string type, HistoryParameters parameters)
        {
            var historyExtension = GetFeature<IHistoryService>();
            var snapshot = await historyExtension.History(type, parameters).ConfigureAwait(false);
            return await CreateSnapshotResponse(snapshot).ConfigureAwait(false);
        }

        public async Task<FhirResponse> History(IKey key, HistoryParameters parameters)
        {
            var storageService = GetFeature<IResourceStorageService>();
            if (await storageService.Get(key).ConfigureAwait(false) == null)
            {
                return Respond.NotFound(key);
            }

            var historyExtension = GetFeature<IHistoryService>();
            var snapshot = await historyExtension.History(key, parameters).ConfigureAwait(false);
            return await CreateSnapshotResponse(snapshot).ConfigureAwait(false);
        }

        public Task<FhirResponse> Mailbox(Bundle bundle, Binary body) => throw new NotImplementedException();

        public Task<FhirResponse> Put(IKey key, Resource resource)
        {
            Validate.HasResourceId(resource);
            Validate.IsResourceIdEqual(key, resource);
            return Put(Entry.Put(key, resource));
        }

        public Task<FhirResponse> Put(Entry entry)
        {
            Validate.Key(entry.Key);
            Validate.ResourceType(entry.Key, entry.Resource);
            Validate.HasTypeName(entry.Key);
            Validate.HasResourceId(entry.Key);

            return Transaction(entry);
        }

        public async Task<FhirResponse> Read(IKey key, ConditionalHeaderParameters parameters = null)
        {
            ValidateKey(key);
            var entry = await GetFeature<IResourceStorageService>().Get(key).ConfigureAwait(false);
            return _responseFactory.GetFhirResponse(entry, key, parameters);
        }

        public async Task<FhirResponse> ReadMeta(IKey key)
        {
            ValidateKey(key);
            var entry = await GetFeature<IResourceStorageService>().Get(key).ConfigureAwait(false);
            return _responseFactory.GetMetadataResponse(entry, key);
        }

        public async Task<FhirResponse> Search(string type, SearchParams searchCommand, int pageIndex = 0)
        {
            var searchService = GetFeature<ISearchService>();
            var snapshot = await searchService.GetSnapshot(type, searchCommand).ConfigureAwait(false);
            return await CreateSnapshotResponse(snapshot, pageIndex).ConfigureAwait(false);
        }

        public async Task<FhirResponse> Transaction(params Entry[] interactions)
        {
            var transactionExtension = GetFeature<ITransactionService>();
            var responses = await transactionExtension.HandleTransaction(interactions, _interactionHandler).ConfigureAwait(false);
            return _responseFactory.GetFhirResponse(responses, Bundle.BundleType.TransactionResponse);
        }

        public async Task<FhirResponse> Transaction(Bundle bundle)
        {
            var transactionExtension = GetFeature<ITransactionService>();
            var responses = await transactionExtension.HandleTransaction(bundle, _interactionHandler).ConfigureAwait(false);
            return _responseFactory.GetFhirResponse(responses, Bundle.BundleType.TransactionResponse);
        }

        public async Task<FhirResponse> Update(IKey key, Resource resource) =>
            key.HasVersionId()
                ? await VersionSpecificUpdate(key, resource).ConfigureAwait(false)
                : await Put(key, resource).ConfigureAwait(false);

        public Task<FhirResponse> ValidateOperation(IKey key, Resource resource)
        {
            if (resource == null)
            {
                throw Error.BadRequest("Validate needs a Resource in the body payload");
            }

            Validate.ResourceType(key, resource);

            var outcome = Validate.AgainstSchema(resource);
            return Task.FromResult(
                outcome == null ? Respond.WithCode(HttpStatusCode.OK) : Respond.WithResource(422, outcome));
        }

        public async Task<FhirResponse> VersionRead(IKey key)
        {
            ValidateKey(key, true);
            var entry = await GetFeature<IResourceStorageService>().Get(key).ConfigureAwait(false);
            return _responseFactory.GetFhirResponse(entry, key);
        }

        public async Task<FhirResponse> VersionSpecificUpdate(IKey versionedKey, Resource resource)
        {
            Validate.HasTypeName(versionedKey);
            Validate.HasVersion(versionedKey);
            var key = versionedKey.WithoutVersion();
            var current = await GetFeature<IResourceStorageService>().Get(key).ConfigureAwait(false);
            Validate.IsSameVersion(current.Key, versionedKey);
            return await Put(key, resource).ConfigureAwait(false);
        }

        public async Task<FhirResponse> Everything(IKey key)
        {
            var searchService = GetFeature<ISearchService>();
            var snapshot = await searchService.GetSnapshotForEverything(key).ConfigureAwait(false);
            return await CreateSnapshotResponse(snapshot).ConfigureAwait(false);
        }

        public async Task<FhirResponse> Document(IKey key)
        {
            Validate.HasResourceType(key, ResourceType.Composition);

            var searchCommand = new SearchParams();
            searchCommand.Add("_id", key.ResourceId);
            var includes = new List<string>
            {
                "Composition:subject",
                "Composition:author",
                "Composition:attester" //Composition.attester.party
                ,
                "Composition:custodian",
                "Composition:eventdetail" //Composition.event.detail
                ,
                "Composition:encounter",
                "Composition:entry" //Composition.section.entry
            };
            foreach (var inc in includes)
            {
                searchCommand.Include.Add((inc, IncludeModifier.None));
            }

            return await Search(key.TypeName, searchCommand).ConfigureAwait(false);
        }

        private static void ValidateKey(IKey key, bool withVersion = false)
        {
            Validate.HasTypeName(key);
            Validate.HasResourceId(key);
            if (withVersion)
            {
                Validate.HasVersion(key);
            }
            else
            {
                Validate.HasNoVersion(key);
            }

            Validate.Key(key);
        }

        private async Task<FhirResponse> CreateSnapshotResponse(Snapshot snapshot, int pageIndex = 0)
        {
            var pagingExtension = FindExtension<IPagingService>();
            if (pagingExtension == null)
            {
                var bundle = new Bundle { Type = snapshot.Type, Total = snapshot.Count };
                var resourceStorage = FindExtension<IResourceStorageService>();
                bundle.Append(await resourceStorage.Get(snapshot.Keys).ConfigureAwait(false));
                return _responseFactory.GetFhirResponse(bundle);
            }
            else
            {
                var pagination = await pagingExtension.StartPagination(snapshot).ConfigureAwait(false);
                var bundle = await pagination.GetPage(pageIndex).ConfigureAwait(false);
                return _responseFactory.GetFhirResponse(bundle);
            }
        }

        private T GetFeature<T>()
            where T : IFhirServiceExtension =>
            FindExtension<T>() ?? throw new NotSupportedException($"Feature {typeof(T)} not supported");

        private async Task<Entry> Store(Entry entry)
        {
            var result = await GetFeature<IResourceStorageService>().Add(entry).ConfigureAwait(false);
            await _serviceListener.Inform(entry).ConfigureAwait(false);
            return result;
        }
    }
}