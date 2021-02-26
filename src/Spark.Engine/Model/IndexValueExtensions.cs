namespace Spark.Engine.Model
{
    using System.Collections.Generic;
    using System.Linq;

    public static class IndexValueExtensions
    {
        public static IEnumerable<IndexValue> IndexValues(this IndexValue root)
        {
            return root.Values.Where(v => v is IndexValue).Select(v => (IndexValue)v);
        }
    }
}