using System.Net.Http.Headers;

namespace Spark.Engine.Extensions
{
    public static class ETag
    {
        public static EntityTagHeaderValue Create(string value)
        {
            var tag = "\"" + value + "\"";
            return new EntityTagHeaderValue(tag, true);
        }
    }
}
