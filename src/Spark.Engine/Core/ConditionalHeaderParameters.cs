using System;
using System.Collections.Generic;
using System.Net.Http;
using Spark.Engine.Extensions;
#if NETSTANDARD2_0
using Microsoft.AspNetCore.Http;
#endif

namespace Spark.Engine.Core
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Net.Http.Headers;

    public class ConditionalHeaderParameters
    {
        public ConditionalHeaderParameters(HttpRequest request)
        {
            IfNoneMatchTags = request.Headers[HeaderNames.IfNoneMatch];
            foreach (var stringValue in request.Headers[HeaderNames.IfModifiedSince])
            {
                if (DateTimeOffset.TryParse(stringValue, out var result))
                {
                    IfModifiedSince = result;
                    break;
                }
            }
        }

        public IEnumerable<string> IfNoneMatchTags { get; }
        public DateTimeOffset? IfModifiedSince { get; }
    }
}