﻿using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using Spark.Engine.Core;

namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System.Threading.Tasks;
    using Extensions;

    public class TransactionService : ITransactionService
    {
        private readonly ILocalhost localhost;
        private readonly ITransfer transfer;
        private readonly ISearchService searchService;

        public TransactionService(ILocalhost localhost, ITransfer transfer, ISearchService searchService)
        {
            this.localhost = localhost;
            this.transfer = transfer;
            this.searchService = searchService;
        }

        public Task<IList<Tuple<Entry, FhirResponse>>> HandleTransaction(IList<Entry> interactions, IInteractionHandler interactionHandler)
        {
            if (interactionHandler == null)
            {
                throw new InvalidOperationException("Unable to run transaction operation");
            }

            return HandleTransaction(interactions, interactionHandler, null);
        }

        public async Task<FhirResponse> HandleTransaction(ResourceManipulationOperation operation, IInteractionHandler interactionHandler)
        {
            IList<Entry> interactions = operation.GetEntries().ToList();

            FhirResponse response = null;
            foreach (Entry interaction in interactions)
            {
                var handleInteraction = await interactionHandler.HandleInteraction(interaction).ConfigureAwait(false);
                response = MergeFhirResponse(response, handleInteraction);
                if (!response.IsValid) throw new Exception();
                interaction.Resource = response.Resource;
            }

            transfer.Externalize(interactions);

            return response;
        }

        private static FhirResponse MergeFhirResponse(FhirResponse previousResponse, FhirResponse response)
        {
            //CCR: How to handle responses?
            //Currently we assume that all FhirResponses from one ResourceManipulationOperation should be equivalent - kind of hackish
            if (previousResponse == null)
                return response;
            if (!response.IsValid)
                return response;
            if (response.StatusCode != previousResponse.StatusCode)
                throw new Exception("Incompatible responses");
            if (response.Key != null && previousResponse.Key != null && response.Key.Equals(previousResponse.Key) == false)
                throw new Exception("Incompatible responses");
            if ((response.Key != null && previousResponse.Key == null) || (response.Key == null && previousResponse.Key != null))
                throw new Exception("Incompatible responses");
            return response;
        }

        private void AddMappingsForOperation(Mapper<string, IKey> mapper, ResourceManipulationOperation operation, IList<Entry> interactions)
        {
            if (mapper == null)
                return;
            if (interactions.Count == 1)
            {
                Entry entry = interactions.First();
                if (!entry.Key.Equals(operation.OperationKey))
                {
                    if (localhost.GetKeyKind(operation.OperationKey) == KeyKind.Temporary)
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

        public async Task<IList<Tuple<Entry, FhirResponse>>> HandleTransaction(Bundle bundle, IInteractionHandler interactionHandler)
        {
            if (interactionHandler == null)
            {
                throw new InvalidOperationException("Unable to run transaction operation");
            }

            var entries = new List<Entry>();
            Mapper<string, IKey> mapper = new Mapper<string, IKey>();

            foreach (var operation in bundle.Entry.Select(
                e => ResourceManipulationOperationFactory.GetManipulationOperation(e, localhost, searchService)))
            {
                IList<Entry> atomicOperations = (await operation.ConfigureAwait(false)).GetEntries().ToList();
                AddMappingsForOperation(mapper, await operation.ConfigureAwait(false), atomicOperations);
                entries.AddRange(atomicOperations);
            }

            return await HandleTransaction(entries, interactionHandler, mapper).ConfigureAwait(false);
        }

        private async Task<IList<Tuple<Entry, FhirResponse>>> HandleTransaction(IList<Entry> interactions, IInteractionHandler interactionHandler, Mapper<string, IKey> mapper)
        {
            List<Tuple<Entry, FhirResponse>> responses = new List<Tuple<Entry, FhirResponse>>();

            await transfer.Internalize(interactions, mapper).ConfigureAwait(false);

            foreach (Entry interaction in interactions)
            {
                FhirResponse response = await interactionHandler.HandleInteraction(interaction).ConfigureAwait(false);
                if (!response.IsValid) throw new Exception();
                interaction.Resource = response.Resource;
                response.Resource = null;

                responses.Add(new Tuple<Entry, FhirResponse>(interaction, response)); //CCR: How to handle responses for transactions?
                                                                                      //The specifications says only one response should be sent per EntryComponent,
                                                                                      //but one EntryComponent might correpond to multiple atomic entries (Entry)
                                                                                      //Example: conditional delete
            }

            transfer.Externalize(interactions);
            return responses;
        }
    }
}