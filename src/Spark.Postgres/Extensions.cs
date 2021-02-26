namespace Spark.Postgres
{
    using System.Collections.Generic;

    internal static class Extensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new(source);
        }
    }
}