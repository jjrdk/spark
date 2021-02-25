namespace Spark.Postgres
{
    using Marten;

    public class FhirRegistry : MartenRegistry
    {
        public FhirRegistry()
        {
            For<EntryEnvelope>()
                .Index(x => x.Id)
                .Duplicate(x => x.ResourceType)
                .Duplicate(x => x.Resource.Id)
                .Duplicate(x => x.Resource.VersionId)
                .Duplicate(x => x.ResourceKey)
                .Duplicate(x => x.Deleted)
                .Duplicate(x => x.IsPresent)
                .Index(x => x.When)
                .GinIndexJsonData();
            For<IndexEntry>()
                .Identity(x => x.Id)
                .Index(x => x.Id)
                .Duplicate(x => x.ResourceType)
                .GinIndexJsonData();
        }
    }
}
