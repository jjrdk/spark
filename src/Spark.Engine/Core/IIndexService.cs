namespace Spark.Engine.Core
{
    using System.Threading.Tasks;
    using Hl7.Fhir.Model;
    using Model;
    using Task = System.Threading.Tasks.Task;

    public interface IIndexService
    {
        Task Process(Entry entry);

        Task<IndexValue> IndexResource(Resource resource, IKey key);
    }
}
