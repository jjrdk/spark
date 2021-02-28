// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Service
{
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Core;
    using Extensions;
    using FhirResponseFactory;
    using FhirServiceExtensions;
    using Hl7.Fhir.Model;
    using Model.Patch;
    using Task = System.Threading.Tasks.Task;

    internal class DefaultInteractionHandler : IInteractionHandler
    {
        private readonly IResourceStorageService _storageService;
        private readonly ICompositeServiceListener _serviceListener;
        private readonly IFhirResponseFactory _responseFactory;

        public DefaultInteractionHandler(
            IResourceStorageService storageService,
            ICompositeServiceListener serviceListener,
            IFhirResponseFactory responseFactory)
        {
            _storageService = storageService;
            _serviceListener = serviceListener;
            _responseFactory = responseFactory;
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
                    {
                        var current = await _storageService.Get(interaction.Key.WithoutVersion()).ConfigureAwait(false);
                        return current != null && current.IsPresent
                            ? await Delete(interaction).ConfigureAwait(false)
                            : Respond.WithCode(HttpStatusCode.NotFound);
                    }
                case Bundle.HTTPVerb.PATCH:
                    {
                        var current = await _storageService.Get(interaction.Key.WithoutVersion()).ConfigureAwait(false);
                        if (current == null)
                        {
                            return new FhirResponse(HttpStatusCode.BadRequest);
                        }

                        if (interaction.Resource is Parameters patch)
                        {
                            var applier = new PatchApplier();
                            applier.Apply(current.Resource as Patient, patch);
                            return new FhirResponse(HttpStatusCode.OK, current.Resource);
                        }
                        else
                        {
                            return new FhirResponse(HttpStatusCode.BadRequest);
                        }
                    }
                    break;
                case Bundle.HTTPVerb.GET:
                    return await VersionRead((Key)interaction.Key).ConfigureAwait(false);
                default:
                    return Respond.Success;
            }
        }

        private async Task<FhirResponse> Delete(Entry entry)
        {
            Validate.Key(entry.Key);
            await _serviceListener.Inform(entry).ConfigureAwait(false);
            _ = await _storageService.Add(entry).ConfigureAwait(false);
            return Respond.WithCode(HttpStatusCode.NoContent);
        }

        private async Task<FhirResponse> VersionRead(IKey key)
        {
            ValidateKey(key, true);
            var entry = await _storageService.Get(key).ConfigureAwait(false);
            return _responseFactory.GetFhirResponse(entry, key);
        }

        private async Task<FhirResponse> Put(Entry entry)
        {
            var exists = await _storageService.Exists(entry.Key.WithoutVersion()).ConfigureAwait(false);
            await _serviceListener.Inform(entry).ConfigureAwait(false);
            var result = await _storageService.Add(entry).ConfigureAwait(false);
            return Respond.WithResource(exists ? HttpStatusCode.OK : HttpStatusCode.Created, result);
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

            var result = await _storageService.Add(entry).ConfigureAwait(false);
            await _serviceListener.Inform(entry).ConfigureAwait(false);
            return Respond.WithResource(HttpStatusCode.Created, result);
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
    }
}
