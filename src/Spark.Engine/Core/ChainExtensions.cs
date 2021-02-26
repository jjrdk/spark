namespace Spark.Engine.Core
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ChainExtensions
    {
        public static bool IsEmpty(this IEnumerable<ElementQuery.Segment> chain)
        {
            return chain.Count() == 0;
        }
    }
}