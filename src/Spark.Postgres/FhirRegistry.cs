namespace Spark.Postgres
{
    using Marten;

    public class FhirRegistry : MartenRegistry
    {
        public FhirRegistry()
        {
            For<EntryEnvelope>()
                .Duplicate(x => x.ResourceType)
                .Duplicate(x => x.Resource.Id)
                .Duplicate(x => x.Resource.VersionId)
                .Duplicate(x => x.Method)
                .Duplicate(x => x.State)
                .Index(x => x.When)
                .GinIndexJsonData();
            For<IndexEntry>().Identity(x => x.Id).GinIndexJsonData();
        }
    }
}