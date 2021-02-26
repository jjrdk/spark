/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

namespace Spark.Core
{
    using Spark.Engine.Core;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hl7.Fhir.Rest;

    public interface IFhirIndex
    {
        Task Clean();

        Task<SearchResults> Search(string resource, SearchParams searchCommand);

        Task<Key> FindSingle(string resource, SearchParams searchCommand);

        Task<SearchResults> GetReverseIncludes(IList<IKey> keys, IList<string> revIncludes);
    }
}
