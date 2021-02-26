using System.Threading.Tasks;
using Spark.Engine.Core;

namespace Spark.Engine.Store.Interfaces
{
    public interface IFhirStorePagedReader
    {
        Task<IPageResult<Entry>> ReadAsync(FhirStorePageReaderOptions options = null);
    }
}
