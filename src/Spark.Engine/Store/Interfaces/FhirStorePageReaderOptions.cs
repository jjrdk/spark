namespace Spark.Engine.Store.Interfaces
{
    public class FhirStorePageReaderOptions
    {
        public int PageSize { get; set; } = 100;

        // TODO: add criteria?
        // TODO: add sorting?
    }
}