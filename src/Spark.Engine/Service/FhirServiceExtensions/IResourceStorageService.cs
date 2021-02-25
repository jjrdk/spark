namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Spark.Engine.Core;

    public interface IResourceStorageService : IFhirServiceExtension
    {
        Task<Entry> Get(IKey key);

        Task<Entry> Add(Entry entry);

        Task<IList<Entry>> Get(IEnumerable<string> localIdentifiers, string sortby = null);
    }
}
