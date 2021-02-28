// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;

    public interface IResourceStorageService : IFhirServiceExtension
    {
        Task<bool> Exists(IKey key);

        Task<Entry> Get(IKey key);

        Task<Entry> Add(Entry entry);

        Task<IList<Entry>> Get(IEnumerable<string> localIdentifiers, string sortby = null);
    }
}