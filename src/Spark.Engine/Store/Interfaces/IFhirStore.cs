namespace Spark.Engine.Store.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Spark.Engine.Core;

    public interface IFhirStore
    {
        Task Add(Entry entry);

        Task<Entry> Get(IKey key);

        Task<IList<Entry>> Get(IEnumerable<IKey> localIdentifiers);
    }
}