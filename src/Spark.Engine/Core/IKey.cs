namespace Spark.Engine.Core
{
    public interface IKey 
    {
        string Base { get; set; }
        string TypeName { get; set; }
        string ResourceId { get; set; }
        string VersionId { get; set; }
    }
}