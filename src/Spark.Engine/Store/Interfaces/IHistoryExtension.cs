using Spark.Engine.Core;

namespace Spark.Engine.Store.Interfaces
{
    using System.Threading.Tasks;
    using Service.FhirServiceExtensions;

    public interface IHistoryStore : IFhirServiceExtension
    {
        Task<Snapshot> History(string typename, HistoryParameters parameters);
        Task<Snapshot> History(IKey key, HistoryParameters parameters);
        Task<Snapshot> History(HistoryParameters parameters);
    }
}