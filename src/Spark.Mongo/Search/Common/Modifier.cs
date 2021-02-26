namespace Spark.Mongo.Search.Common
{
    using System;

    public static class Modifier
    {
        [Obsolete]
        public const string
            BEFORE = "before",
            AFTER = "after",
            SEPARATOR = ":";

        public const string
            EXACT = "exact",
            CONTAINS = "contains",
            PARTIAL = "partial",
            TEXT = "text",
            CODE = "code",
            ANYNAMESPACE = "anyns",
            MISSING = "missing",
            BELOW = "below",
            ABOVE = "above",
            NOT = "not",
            NONE = "";
    }
}