namespace Spark.Mongo.Search.Common
{
    public static class UniversalField
    {
        public const string
            ID = "_id",
            TAG = "_tag";

        public static string[] All = { ID, TAG };
    }
}