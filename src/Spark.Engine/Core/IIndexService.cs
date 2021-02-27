// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Core
{
    using System.Threading.Tasks;
    using Hl7.Fhir.Model;
    using Model;
    using Task = System.Threading.Tasks.Task;

    public interface IIndexService
    {
        Task Process(Entry entry);

        Task<IndexValue> IndexResource(Resource resource, IKey key);
    }
}