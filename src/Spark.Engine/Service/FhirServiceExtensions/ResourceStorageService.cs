namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Spark.Engine.Core;
    using Spark.Engine.Store.Interfaces;

    public class ResourceStorageService : IResourceStorageService
    {
        private readonly ITransfer _transfer;
        private readonly IFhirStore _fhirStore;


        public ResourceStorageService(ITransfer transfer, IFhirStore fhirStore)
        {
            this._transfer = transfer;
            this._fhirStore = fhirStore;
        }

        public async Task<Entry> Get(IKey key)
        {
            var entry = await _fhirStore.Get(key).ConfigureAwait(false);
            if (entry != null)
            {
                _transfer.Externalize(entry);
            }

            return entry;
        }

        public async Task<Entry> Add(Entry entry)
        {
            if (entry.State != EntryState.Internal)
            {
                await _transfer.Internalize(entry).ConfigureAwait(false);
            }

            await _fhirStore.Add(entry).ConfigureAwait(false);
            Entry result;
            if (entry.IsDelete)
            {
                result = entry;
            }
            else
            {
                result = await _fhirStore.Get(entry.Key).ConfigureAwait(false);
            }

            _transfer.Externalize(result);

            return result;
        }

        public async Task<IList<Entry>> Get(IEnumerable<string> localIdentifiers, string sortby = null)
        {
            var results = await _fhirStore.Get(localIdentifiers.Select(k => (IKey)Key.ParseOperationPath(k)))
                .ConfigureAwait(false);
            _transfer.Externalize(results);
            return results;
        }
    }
}
