namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System.Threading.Tasks;
    using Spark.Engine.Core;

    public interface IPagingService : IFhirServiceExtension
    {
        Task<ISnapshotPagination> StartPagination(Snapshot snapshot);
        Task<ISnapshotPagination> StartPagination(string snapshotKey);
    }
}
