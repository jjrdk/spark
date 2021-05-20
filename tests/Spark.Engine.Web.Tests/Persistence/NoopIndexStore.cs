namespace Spark.Engine.Web.Tests.Persistence
{
    using System.Threading.Tasks;
    using Core;
    using Model;
    using Store.Interfaces;

    public class NoopIndexStore : IIndexStore
    {
        /// <inheritdoc />
        public Task Save(IndexValue indexValue) => Task.CompletedTask;

        /// <inheritdoc />
        public Task Delete(Entry entry) => Task.CompletedTask;

        /// <inheritdoc />
        public Task Clean() => Task.CompletedTask;
    }
}