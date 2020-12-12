﻿using System;
using System.Collections.Generic;
using System.Net;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Spark.Core;
using Spark.Engine.Core;
using Spark.Engine.Extensions;
using Spark.Engine.FhirResponseFactory;
using Spark.Engine.Service.FhirServiceExtensions;
using Spark.Engine.Storage;
using Spark.Service;

namespace Spark.Engine.Service
{
    using System.Threading.Tasks;

    public class FhirService : ExtendableWith<IFhirServiceExtension>, IFhirService, IInteractionHandler
    //CCCR: FhirService now implementents InteractionHandler that is used by the TransactionService to actually perform the operation.
    //This creates a circular reference that is solved by sending the handler on each call.
    //A future step might be to split that part into a different service (maybe StorageService?)
    {
        private readonly IFhirResponseFactory _responseFactory;
        private readonly ICompositeServiceListener _serviceListener;

        public FhirService(IFhirServiceExtension[] extensions,
            IFhirResponseFactory responseFactory, //TODO: can we remove this dependency?
            ICompositeServiceListener serviceListener = null) //TODO: can we remove this dependency? - CCR
        {
            this._responseFactory = responseFactory;
            this._serviceListener = serviceListener;

            foreach (IFhirServiceExtension serviceExtension in extensions)
            {
                this.AddExtension(serviceExtension);
            }
        }

        public async Task<FhirResponse> Read(IKey key, ConditionalHeaderParameters parameters = null)
        {
            ValidateKey(key);

            var resourceStorageService = GetFeature<IResourceStorageService>();
            Entry entry = await resourceStorageService.Get(key).ConfigureAwait(false);

            return _responseFactory.GetFhirResponse(entry, key, parameters);
        }

        public async Task<FhirResponse> ReadMeta(IKey key)
        {
            ValidateKey(key);

            var resourceStorageService = GetFeature<IResourceStorageService>();
            Entry entry = await resourceStorageService.Get(key).ConfigureAwait(false);

            return _responseFactory.GetMetadataResponse(entry, key);
        }

        public async Task<FhirResponse> AddMeta(IKey key, Parameters parameters)
        {
            var storageService = GetFeature<IResourceStorageService>();
            Entry entry = await storageService.Get(key).ConfigureAwait(false);

            if (entry != null && entry.IsDeleted() == false)
            {
                entry.Resource.AffixTags(parameters);
                _ = await storageService.Add(entry).ConfigureAwait(false);
            }

            return _responseFactory.GetMetadataResponse(entry, key);
        }

        public async Task<FhirResponse> VersionRead(IKey key)
        {
            ValidateKey(key, true);
            var resourceStorageService = GetFeature<IResourceStorageService>();
            Entry entry = await resourceStorageService.Get(key).ConfigureAwait(false);

            return _responseFactory.GetFhirResponse(entry, key);
        }

        public async Task<FhirResponse> Create(IKey key, Resource resource)
        {
            Validate.Key(key);
            Validate.HasTypeName(key);
            Validate.ResourceType(key, resource);

            Validate.HasNoResourceId(key);
            Validate.HasNoVersion(key);


            Entry result = await Store(Entry.POST(key, resource)).ConfigureAwait(false);

            return Respond.WithResource(HttpStatusCode.Created, result);
        }

        private async Task<FhirResponse> Create(Entry entry)
        {
            Validate.Key(entry.Key);
            Validate.HasTypeName(entry.Key);
            Validate.ResourceType(entry.Key, entry.Resource);

            if (entry.State != EntryState.Internal)
            {
                Validate.HasNoResourceId(entry.Key);
                Validate.HasNoVersion(entry.Key);
            }


            Entry result = await Store(entry).ConfigureAwait(false);

            return Respond.WithResource(HttpStatusCode.Created, result);
        }

        public async Task<FhirResponse> Put(Entry entry)
        {
            Validate.Key(entry.Key);
            Validate.ResourceType(entry.Key, entry.Resource);
            Validate.HasTypeName(entry.Key);
            Validate.HasResourceId(entry.Key);


            var storageService = GetFeature<IResourceStorageService>();
            Entry current = await storageService.Get(entry.Key.WithoutVersion()).ConfigureAwait(false);

            Entry result = await Store(entry).ConfigureAwait(false);

            return Respond.WithResource(current != null ? HttpStatusCode.OK : HttpStatusCode.Created, result);

        }
        public Task<FhirResponse> Put(IKey key, Resource resource)
        {
            Validate.HasResourceId(resource);
            Validate.IsResourceIdEqual(key, resource);
            return Put(Entry.PUT(key, resource));
        }

        public async Task<FhirResponse> ConditionalCreate(IKey key, Resource resource, SearchParams parameters)
        {
            ISearchService searchStore = this.FindExtension<ISearchService>();
            ITransactionService transactionService = this.FindExtension<ITransactionService>();
            if (searchStore == null || transactionService == null)
                throw new NotSupportedException("Operation not supported");

            var resourceManipulationOperation = await ResourceManipulationOperationFactory.CreatePost(resource, key, searchStore, parameters).ConfigureAwait(false);
            return await transactionService.HandleTransaction(resourceManipulationOperation, this).ConfigureAwait(false);
        }

        public async Task<FhirResponse> Everything(IKey key)
        {
            ISearchService searchService = this.GetFeature<ISearchService>();

            Snapshot snapshot = await searchService.GetSnapshotForEverything(key).ConfigureAwait(false);

            return await CreateSnapshotResponse(snapshot).ConfigureAwait(false);
        }

        public Task<FhirResponse> Document(IKey key)
        {
            Validate.HasResourceType(key, ResourceType.Composition);

            var searchCommand = new SearchParams();
            searchCommand.Add("_id", key.ResourceId);
            var includes = new List<string>()
            {
                "Composition:subject"
                , "Composition:author"
                , "Composition:attester" //Composition.attester.party
                , "Composition:custodian"
                , "Composition:eventdetail" //Composition.event.detail
                , "Composition:encounter"
                , "Composition:entry" //Composition.section.entry
            };
            foreach (var inc in includes)
            {
                searchCommand.Include.Add(inc);
            }
            return Search(key.TypeName, searchCommand);
        }

        public async Task<FhirResponse> VersionSpecificUpdate(IKey versionedkey, Resource resource)
        {
            Validate.HasTypeName(versionedkey);
            Validate.HasVersion(versionedkey);
            Key key = versionedkey.WithoutVersion();
            var resourceStorageService = GetFeature<IResourceStorageService>();
            Entry current = await resourceStorageService.Get(key).ConfigureAwait(false);
            Validate.IsSameVersion(current.Key, versionedkey);

            return await Put(key, resource).ConfigureAwait(false);
        }

        public Task<FhirResponse> Update(IKey key, Resource resource)
        {
            return key.HasVersionId() ? this.VersionSpecificUpdate(key, resource)
                : this.Put(key, resource);
        }

        public async Task<FhirResponse> ConditionalUpdate(IKey key, Resource resource, SearchParams _params)
        {
            //if update receives a key with no version how do we handle concurrency?
            ISearchService searchStore = this.FindExtension<ISearchService>();
            ITransactionService transactionService = this.FindExtension<ITransactionService>();
            if (searchStore == null || transactionService == null)
                throw new NotSupportedException("Operation not supported");
            var resourceManipulationOperation = await ResourceManipulationOperationFactory.CreatePut(resource, key, searchStore, _params).ConfigureAwait(false);
            return await transactionService.HandleTransaction(
                resourceManipulationOperation,
                this).ConfigureAwait(false);
        }

        public async Task<FhirResponse> Delete(IKey key)
        {
            Validate.Key(key);
            Validate.HasNoVersion(key);

            var resourceStorage = GetFeature<IResourceStorageService>();

            Entry current = await resourceStorage.Get(key).ConfigureAwait(false);
            if (current != null && current.IsPresent)
            {
                return await Delete(Entry.DELETE(key, DateTimeOffset.UtcNow)).ConfigureAwait(false);
            }
            return Respond.WithCode(HttpStatusCode.NoContent);

        }

        private async Task<FhirResponse> Delete(Entry entry)
        {
            Validate.Key(entry.Key);
            _ = await Store(entry).ConfigureAwait(false);
            return Respond.WithCode(HttpStatusCode.NoContent);
        }

        public async Task<FhirResponse> ConditionalDelete(IKey key, IEnumerable<Tuple<string, string>> parameters)
        {
            ISearchService searchStore = FindExtension<ISearchService>();
            ITransactionService transactionService = FindExtension<ITransactionService>();
            if (searchStore == null || transactionService == null)
                throw new NotSupportedException("Operation not supported");

            var interactions = await ResourceManipulationOperationFactory.CreateDelete(
                key,
                searchStore,
                SearchParams.FromUriParamList(parameters)).ConfigureAwait(false);
            return await transactionService.HandleTransaction(interactions, this)
.ConfigureAwait(false) ?? Respond.WithCode(HttpStatusCode.NotFound);
        }

        public FhirResponse ValidateOperation(IKey key, Resource resource)
        {
            if (resource == null) throw Error.BadRequest("Validate needs a Resource in the body payload");
            Validate.ResourceType(key, resource);

            // DSTU2: validation
            var outcome = Validate.AgainstSchema(resource);

            return outcome == null ? Respond.WithCode(HttpStatusCode.OK) : Respond.WithResource(422, outcome);
        }

        public async Task<FhirResponse> Search(string type, SearchParams searchCommand, int pageIndex = 0)
        {
            ISearchService searchService = this.GetFeature<ISearchService>();

            Snapshot snapshot = await searchService.GetSnapshot(type, searchCommand).ConfigureAwait(false);

            return await CreateSnapshotResponse(snapshot, pageIndex).ConfigureAwait(false);
        }

        private async Task<FhirResponse> CreateSnapshotResponse(Snapshot snapshot, int pageIndex = 0)
        {
            IPagingService pagingExtension = this.FindExtension<IPagingService>();
            IResourceStorageService resourceStorage = this.FindExtension<IResourceStorageService>();
            if (pagingExtension == null)
            {
                Bundle bundle = new Bundle()
                {
                    Type = snapshot.Type,
                    Total = snapshot.Count
                };
                var resources = await resourceStorage.Get(snapshot.Keys).ConfigureAwait(false);
                bundle.Append(resources);
                return _responseFactory.GetFhirResponse(bundle);
            }
            else
            {
                var startPagination = await pagingExtension.StartPagination(snapshot).ConfigureAwait(false);
                Bundle bundle = await startPagination.GetPage(pageIndex).ConfigureAwait(false);
                return _responseFactory.GetFhirResponse(bundle);
            }
        }

        public async Task<FhirResponse> Transaction(Bundle bundle)
        {
            ITransactionService transactionExtension = this.GetFeature<ITransactionService>();
            var handleTransaction = await transactionExtension.HandleTransaction(bundle, this).ConfigureAwait(false);
            return _responseFactory.GetFhirResponse(
                handleTransaction,
                Bundle.BundleType.TransactionResponse);
        }

        public async Task<FhirResponse> History(HistoryParameters parameters)
        {
            IHistoryService historyExtension = this.GetFeature<IHistoryService>();

            var snapshot = await historyExtension.History(parameters).ConfigureAwait(false);
            return await CreateSnapshotResponse(snapshot).ConfigureAwait(false);
        }

        public async Task<FhirResponse> History(string type, HistoryParameters parameters)
        {
            IHistoryService historyExtension = this.GetFeature<IHistoryService>();

            var snapshot = await historyExtension.History(type, parameters).ConfigureAwait(false);
            return await CreateSnapshotResponse(snapshot).ConfigureAwait(false);
        }

        public async Task<FhirResponse> History(IKey key, HistoryParameters parameters)
        {
            IResourceStorageService storageService = GetFeature<IResourceStorageService>();
            if (storageService.Get(key) == null)
            {
                return Respond.NotFound(key);
            }
            IHistoryService historyExtension = this.GetFeature<IHistoryService>();

            var snapshot = await historyExtension.History(key, parameters).ConfigureAwait(false);
            return await CreateSnapshotResponse(snapshot).ConfigureAwait(false);
        }

        public Task<FhirResponse> Mailbox(Bundle bundle, Binary body)
        {
            throw new NotImplementedException();
        }

        public FhirResponse Conformance(string sparkVersion)
        {
            IConformanceService conformanceService = this.GetFeature<IConformanceService>();

            return Respond.WithResource(conformanceService.GetSparkConformance(sparkVersion));
        }

        public async Task<FhirResponse> GetPage(string snapshotkey, int index)
        {
            IPagingService pagingExtension = this.FindExtension<IPagingService>();
            if (pagingExtension == null)
                throw new NotSupportedException("Operation not supported");

            var startPagination = await pagingExtension.StartPagination(snapshotkey).ConfigureAwait(false);
            var responses = await startPagination.GetPage(index).ConfigureAwait(false);
            return _responseFactory.GetFhirResponse(responses);
        }

        public async Task<FhirResponse> HandleInteraction(Entry interaction)
        {
            switch (interaction.Method)
            {
                case Bundle.HTTPVerb.PUT:
                    return await Put(interaction).ConfigureAwait(false);
                case Bundle.HTTPVerb.POST:
                    return await Create(interaction).ConfigureAwait(false);
                case Bundle.HTTPVerb.DELETE:
                    var resourceStorage = GetFeature<IResourceStorageService>();
                    var current = await resourceStorage.Get(interaction.Key.WithoutVersion()).ConfigureAwait(false);
                    if (current != null && current.IsPresent)
                    {
                        return await Delete(interaction).ConfigureAwait(false);
                    }
                    // FIXME: there's no way to distinguish between "successfully deleted"
                    // and "resource not deleted because it doesn't exist" responses, all return NoContent.
                    // Same with Delete method above.
                    return Respond.WithCode(HttpStatusCode.NoContent);
                case Bundle.HTTPVerb.GET:
                    return await VersionRead((Key)interaction.Key).ConfigureAwait(false);
                default:
                    return Respond.Success;
            }
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

        private T GetFeature<T>() where T : IFhirServiceExtension
        {
            //TODO: return 501 - 	Requested HTTP operation not supported?

            T feature = this.FindExtension<T>();
            if (feature == null)
                throw new NotSupportedException("Operation not supported");

            return feature;
        }

        private async Task<Entry> Store(Entry entry)
        {
            var resourceStorageService = GetFeature<IResourceStorageService>();
            Entry result = await resourceStorageService.Add(entry).ConfigureAwait(false);
            await _serviceListener.Inform(entry).ConfigureAwait(false);
            return result;
        }
    }
}