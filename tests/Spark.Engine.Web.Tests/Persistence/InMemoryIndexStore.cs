namespace Spark.Engine.Web.Tests.Persistence
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using Engine.Extensions;
    using Model;
    using Search.ValueExpressionTypes;
    using Store.Interfaces;

    public class InMemoryIndexStore : IIndexStore
    {
        private readonly List<IndexValue> _indexValues = new();

        /// <inheritdoc />
        public Task Save(IndexValue indexValue)
        {
            _indexValues.Add(indexValue);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task Delete(Entry entry)
        {
            _indexValues.RemoveAll(
                x => x.Values.OfType<IndexValue>()
                    .Any(
                        v => v.Name == "internal_id"
                             && v.Values.OfType<StringValue>()
                                 .All(sv => sv.Value == entry.Key.WithoutVersion().ToStorageKey())));
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task Clean()
        {
            return Task.CompletedTask;
        }
    }
}