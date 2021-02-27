// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

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