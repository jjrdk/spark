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

    public class TransactionService : ITransactionService
    {
        private readonly ILocalhost _localhost;
        private readonly ISearchService _searchService;
        private readonly ITransfer _transfer;

        public TransactionService(ILocalhost localhost, ITransfer transfer, ISearchService searchService)
        {
            _localhost = localhost;
            _transfer = transfer;
            _searchService = searchService;
        }

        public async Task<IList<Tuple<Entry, FhirResponse>>> HandleTransaction(
            IList<Entry> interactions,
            IInteractionHandler interactionHandler) =>
            interactionHandler == null
                ? throw new InvalidOperationException("Unable to run transaction operation")
                : await HandleTransaction(interactions, interactionHandler, null).ConfigureAwait(false);

        public Task<FhirResponse> HandleTransaction(
            ResourceManipulationOperation operation,
            IInteractionHandler interactionHandler) =>
            HandleOperation(operation, interactionHandler);

        public async Task<IList<Tuple<Entry, FhirResponse>>> HandleTransaction(
            Bundle bundle,
            IInteractionHandler interactionHandler)
        {
            if (interactionHandler == null)
            {
                throw new InvalidOperationException("Unable to run transaction operation");
            }

            var entries = new List<Entry>();
            var mapper = new Mapper<string, IKey>();

            foreach (var task in bundle.Entry.Select(
                e => ResourceManipulationOperationFactory.GetManipulationOperation(e, _localhost, _searchService)))
            {
                var operation = await task.ConfigureAwait(false);
                IList<Entry> atomicOperations = operation.GetEntries().ToList();
                AddMappingsForOperation(mapper, operation, atomicOperations);
                entries.AddRange(atomicOperations);
            }

            return await HandleTransaction(entries, interactionHandler, mapper).ConfigureAwait(false);
        }

        public async Task<FhirResponse> HandleOperation(
            ResourceManipulationOperation operation,
            IInteractionHandler interactionHandler,
            Mapper<string, IKey> mapper = null)
        {
            IList<Entry> interactions = operation.GetEntries().ToList();
            if (mapper != null)
            {
                _transfer.Internalize(interactions, mapper);
            }

            FhirResponse response = null;
            foreach (var interaction in interactions)
            {
                response = MergeFhirResponse(
                    response,
                    await interactionHandler.HandleInteraction(interaction).ConfigureAwait(false));
                if (!response.IsValid)
                {
                    throw new Exception();
                }

                interaction.Resource = response.Resource;
            }

            _transfer.Externalize(interactions);

            return response;
        }

        private FhirResponse MergeFhirResponse(FhirResponse previousResponse, FhirResponse response)
        {
            //CCR: How to handle responses?
            //Currently we assume that all FhirResponses from one ResourceManipulationOperation should be equivalent - kind of hackish
            if (previousResponse == null)
            {
                return response;
            }

            if (!response.IsValid)
            {
                return response;
            }

            if (response.StatusCode != previousResponse.StatusCode)
            {
                throw new Exception("Incompatible responses");
            }

            if (response.Key != null
                && previousResponse.Key != null
                && response.Key.Equals(previousResponse.Key) == false)
            {
                throw new Exception("Incompatible responses");
            }

            return response.Key != null && previousResponse.Key == null
                   || response.Key == null && previousResponse.Key != null
                ? throw new Exception("Incompatible responses")
                : response;
        }

        private void AddMappingsForOperation(
            Mapper<string, IKey> mapper,
            ResourceManipulationOperation operation,
            IList<Entry> interactions)
        {
            if (mapper == null)
            {
                return;
            }

            if (interactions.Count() == 1)
            {
                var entry = interactions.First();
                if (!entry.Key.Equals(operation.OperationKey))
                {
                    if (_localhost.GetKeyKind(operation.OperationKey) == KeyKind.Temporary)
                    {
                        mapper.Remap(operation.OperationKey.ResourceId, entry.Key.WithoutVersion());
                    }
                    else
                    {
                        mapper.Remap(operation.OperationKey.ToString(), entry.Key.WithoutVersion());
                    }
                }
            }
        }

        private async Task<IList<Tuple<Entry, FhirResponse>>> HandleTransaction(
            IList<Entry> interactions,
            IInteractionHandler interactionHandler,
            Mapper<string, IKey> mapper)
        {
            var responses = new List<Tuple<Entry, FhirResponse>>();

            _transfer.Internalize(interactions, mapper);

            foreach (var interaction in interactions)
            {
                var response = await interactionHandler.HandleInteraction(interaction).ConfigureAwait(false);
                if (!response.IsValid)
                {
                    throw new Exception();
                }

                interaction.Resource = response.Resource;
                response.Resource = null;

                responses.Add(
                    new Tuple<Entry, FhirResponse>(
                        interaction,
                        response)); //CCR: How to handle responses for transactions? 
                //The specifications says only one response should be sent per EntryComponent, 
                //but one EntryComponent might correpond to multiple atomic entries (Entry)
                //Example: conditional delete
            }

            _transfer.Externalize(interactions);
            return responses;
        }
    }
}