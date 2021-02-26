namespace Spark.Engine.Store.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPageResult<out T>
    {
        long TotalRecords { get; }

        long TotalPages { get; }

        Task IterateAllPagesAsync(Func<IReadOnlyList<T>, Task> callback);
    }
}