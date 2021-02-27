// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System.Threading.Tasks;
    using Core;
    using Hl7.Fhir.Rest;

    public interface ISearchService : IFhirServiceExtension
    {
        Task<Snapshot> GetSnapshot(string type, SearchParams searchCommand);

        Task<Snapshot> GetSnapshotForEverything(IKey key);

        Task<IKey> FindSingle(string type, SearchParams searchCommand);

        Task<IKey> FindSingleOrDefault(string type, SearchParams searchCommand);

        Task<SearchResults> GetSearchResults(string type, SearchParams searchCommand);
    }
}