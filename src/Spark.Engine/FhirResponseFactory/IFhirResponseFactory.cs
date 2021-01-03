namespace Spark.Engine.FhirResponseFactory
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Hl7.Fhir.Model;

    public interface IFhirResponseFactory
    {
        FhirResponse GetFhirResponse(Entry entry, IKey key = null, IEnumerable<object> parameters = null);
        FhirResponse GetFhirResponse(Entry entry, IKey key = null, params object[] parameters);
        FhirResponse GetMetadataResponse(Entry entry, IKey key = null);
        FhirResponse GetFhirResponse(Bundle bundle);
        FhirResponse GetFhirResponse(IEnumerable<Tuple<Entry, FhirResponse>> responses, Bundle.BundleType bundleType);
    }
}