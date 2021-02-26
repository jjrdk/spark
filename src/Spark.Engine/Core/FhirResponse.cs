namespace Spark.Engine.Core
{
    using System.Net;
    using Hl7.Fhir.Model;

    public class FhirResponse
    {
        public HttpStatusCode StatusCode;
        public IKey Key;
        public Resource Resource;

        public FhirResponse(HttpStatusCode code, IKey key, Resource resource)
        {
            this.StatusCode = code;
            this.Key = key;
            this.Resource = resource;
        }

        public FhirResponse(HttpStatusCode code, Resource resource)
        {
            this.StatusCode = code;
            this.Key = null;
            this.Resource = resource;
        }

        public FhirResponse(HttpStatusCode code)
        {
            this.StatusCode = code;
            this.Key = null;
            this.Resource = null;
        }

        public bool IsValid
        {
            get
            {
                var code = (int)this.StatusCode;
                return code <= 300;
            }
        }

        public bool HasBody => Resource != null;

        public override string ToString()
        {
            var details = Resource != null ? $"({Resource.TypeName})" : null;
            var location = Key?.ToString();
            return $"{(int) StatusCode}: {StatusCode} {details} ({location})";
        }
    }
}