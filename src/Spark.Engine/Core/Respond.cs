namespace Spark.Engine.Core
{
    using System.Net;
    using Extensions;
    using Hl7.Fhir.Model;

    // This class serves instances of "Response"
    public static class Respond
    {
        public static FhirResponse WithCode(HttpStatusCode code)
        {
            return new FhirResponse(code, null);
        }

        public static FhirResponse WithError(HttpStatusCode code, string message, params object[] args)
        {
            var outcome = new OperationOutcome();
            outcome.AddError(string.Format(message, args));
            return new FhirResponse(code, outcome);
        }

        public static FhirResponse WithResource(int code, Resource resource)
        {
            return new FhirResponse((HttpStatusCode)code, resource);
        }

        public static FhirResponse WithResource(Resource resource)
        {
            return new FhirResponse(HttpStatusCode.OK, resource);
        }

        public static FhirResponse WithBundle(Bundle bundle)
        {
            return new FhirResponse(HttpStatusCode.OK, bundle);
        }

        public static FhirResponse WithMeta(Meta meta)
        {
            var parameters = new Parameters();
            parameters.Add(nameof(Meta), meta);
            return WithResource(parameters);
        }

        public static FhirResponse WithMeta(Entry entry)
        {
            return entry.Resource?.Meta != null
                ? WithMeta(entry.Resource.Meta)
                : WithError(
                    HttpStatusCode.InternalServerError,
                    "Could not retrieve meta. Meta was not present on the resource");
        }

        public static FhirResponse WithResource(HttpStatusCode code, Entry entry)
        {
            return new FhirResponse(code, entry.Key, entry.Resource);
        }

        public static FhirResponse WithResource(Entry entry)
        {
            return new FhirResponse(HttpStatusCode.OK, entry.Key, entry.Resource);
        }

        public static FhirResponse NotFound(IKey key)
        {
            return key.VersionId == null
                ? WithError(
                    HttpStatusCode.NotFound,
                    "No {0} resource with id {1} was found.",
                    key.TypeName,
                    key.ResourceId)
                : WithError(
                    HttpStatusCode.NotFound,
                    "There is no {0} resource with id {1}, or there is no version {2}",
                    key.TypeName,
                    key.ResourceId,
                    key.VersionId);
            // For security reasons (leakage): keep message in sync with Error.NotFound(key)
        }

        public static FhirResponse Gone(Entry entry)
        {

            var message =
                $"A {entry.Key.TypeName} resource with id {entry.Key.ResourceId} existed, but was deleted on {entry.When} (version {entry.Key.ToRelativeUri()}).";

            return WithError(HttpStatusCode.Gone, message);
        }

        public static FhirResponse Success => new FhirResponse(HttpStatusCode.OK);
    }
}
