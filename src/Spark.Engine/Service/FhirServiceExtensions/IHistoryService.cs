namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System.Threading.Tasks;
    using Spark.Engine.Core;

    internal interface IHistoryService : IFhirServiceExtension
    {
        Task<Snapshot> History(string typename, HistoryParameters parameters);

        Task<Snapshot> History(IKey key, HistoryParameters parameters);

        Task<Snapshot> History(HistoryParameters parameters);
    }
}
