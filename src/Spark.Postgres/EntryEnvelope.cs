namespace Spark.Postgres
{
    using System;
    using Engine.Core;
    using Hl7.Fhir.Model;

    public class EntryEnvelope
    {
        public string Id { get; init; }

        public string ResourceType { get; init; }

        public EntryState State { get; init; }
        public IKey Key { get; init; }
        public Bundle.HTTPVerb Method { get; init; }
        public DateTimeOffset? When { get; init; }
        public Resource Resource { get; init; }
    }
}