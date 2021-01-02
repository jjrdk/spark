namespace Spark.Postgres
{
    using System;
    using System.Threading.Tasks;
    using Engine.Core;
    using Engine.Service;

    public class IndexListener : IServiceListener
    {
        private readonly IIndexService _index;

        public IndexListener(IIndexService index)
        {
            _index = index;
        }

        /// <inheritdoc />
        public Task Inform(Uri location, Entry interaction)
        {
            return _index.Process(interaction);
        }
    }
}