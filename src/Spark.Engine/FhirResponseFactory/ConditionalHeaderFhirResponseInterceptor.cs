using System.Linq;
using System.Net;
using Spark.Engine.Core;
using Spark.Engine.Extensions;
using Spark.Engine.Interfaces;

namespace Spark.Engine.FhirResponseFactory
{
    public class ConditionalHeaderFhirResponseInterceptor : IFhirResponseInterceptor
    {
        public bool CanHandle(object input)
        {
            return input is ConditionalHeaderParameters;
        }

        private ConditionalHeaderParameters ConvertInput(object input)
        {
            return input as ConditionalHeaderParameters;
        }

        public FhirResponse GetFhirResponse(Entry entry, object input)
        {
            var parameters = ConvertInput(input);
            if (parameters == null)
            {
                return null;
            }

            var matchTags = parameters.IfNoneMatchTags.Any() ? parameters.IfNoneMatchTags.Any(t => t == ETag.Create(entry.Key.VersionId).Tag) : (bool?)null;
            var matchModifiedDate = parameters.IfModifiedSince.HasValue
                ? parameters.IfModifiedSince.Value < entry.Resource.Meta.LastUpdated
                : (bool?) null;

            if (!matchTags.HasValue  && !matchModifiedDate.HasValue)
            {
                return null;
            }

            return (matchTags ?? true) && (matchModifiedDate ?? true) ? Respond.WithCode(HttpStatusCode.NotModified) : null;
        }
    }
}