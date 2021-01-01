namespace Spark.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ConditionalHeaderParameters
    {
        public ConditionalHeaderParameters(IEnumerable<string> ifNoneMatchTags, DateTimeOffset? ifModifiedSince)
        {
            IfModifiedSince = ifModifiedSince;
            IfNoneMatchTags = ifNoneMatchTags.ToArray();
        }

        public IEnumerable<string> IfNoneMatchTags { get; }
        public DateTimeOffset? IfModifiedSince { get; }
    }
}