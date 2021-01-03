namespace Spark.Engine.Search.Model
{
    public enum Modifier
    {
        UNKNOWN = 0,
        EXACT = 1,
        PARTIAL = 2,
        TEXT = 3,
        CONTAINS = 4,
        ANYNAMESPACE = 5,
        MISSING = 6,
        BELOW = 7,
        ABOVE = 8,
        IN = 9,
        NOT_IN = 10,
        TYPE = 11,
        NONE = 12
    }
}
