namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System.Threading.Tasks;
    using Spark.Engine.Core;
    using Spark.Engine.Store.Interfaces;

    public class HistoryService : IHistoryService
    {
        private readonly IHistoryStore _historyStore;

        public HistoryService(IHistoryStore historyStore)
        {
            this._historyStore = historyStore;
        }

        public async Task<Snapshot> History(string typename, HistoryParameters parameters)
        {
            return await _historyStore.History(typename, parameters).ConfigureAwait(false);
        }

        public async Task<Snapshot> History(IKey key, HistoryParameters parameters)
        {

            return await _historyStore.History(key, parameters).ConfigureAwait(false);
        }

        public async Task<Snapshot> History(HistoryParameters parameters)
        {
            return await _historyStore.History(parameters).ConfigureAwait(false);
        }

    }
}