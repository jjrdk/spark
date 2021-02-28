// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Store.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;

    public interface IFhirStore
    {
        Task Add(Entry entry);

        Task<Entry> Get(IKey key);

        Task<IList<Entry>> Get(IEnumerable<IKey> localIdentifiers);
        Task<bool> Exists(IKey key);
    }
}