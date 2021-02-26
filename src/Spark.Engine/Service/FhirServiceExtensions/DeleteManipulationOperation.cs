﻿using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Spark.Engine.Core;

namespace Spark.Engine.Service.FhirServiceExtensions
{
    public static partial class ResourceManipulationOperationFactory
    {
        private class DeleteManipulationOperation : ResourceManipulationOperation
        {
            public DeleteManipulationOperation(Resource resource, IKey operationKey, SearchResults searchResults, SearchParams searchCommand = null)
                : base(resource, operationKey, searchResults, searchCommand)
            {
            }

            public static Uri ReadSearchUri(Bundle.EntryComponent entry)
            {
                return entry.Request != null
                    ? new Uri(entry.Request.Url, UriKind.RelativeOrAbsolute)
                    : null;
            }

            protected override IEnumerable<Entry> ComputeEntries()
            {
                if (SearchResults != null)
                {
                    foreach (var localKeyValue in SearchResults)
                    {
                        yield return Entry.Delete(Key.ParseOperationPath(localKeyValue), DateTimeOffset.UtcNow);
                    }
                }
                else
                {
                    yield return Entry.Delete(OperationKey, DateTimeOffset.UtcNow);
                }
            }
        }
    }
}