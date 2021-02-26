namespace Spark.Engine.Store.Interfaces
{
    using System.Threading.Tasks;
    using Spark.Engine.Core;

    public interface ISnapshotStore
    {
        Task AddSnapshot(Snapshot snapshot);

        Task<Snapshot> GetSnapshot(string snapshotId);
    }
}
