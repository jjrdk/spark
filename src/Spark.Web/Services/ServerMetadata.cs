// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Web.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Engine.Service.FhirServiceExtensions;
    using Hl7.Fhir.Rest;

    public partial class ServerMetadata
    {
        private readonly ISearchService _searchService;

        public ServerMetadata(ISearchService searchService) => _searchService = searchService;

        public async Task<List<ResourceStat>> GetResourceStats()
        {
            var stats = new List<ResourceStat>();
            var names = Hl7.Fhir.Model.ModelInfo.SupportedResources;

            foreach (var name in names)
            {
                var search = await _searchService.GetSnapshot(name, new SearchParams { Summary = SummaryType.Count })
                    .ConfigureAwait(false);
                stats.Add(new ResourceStat { ResourceName = name, Count = search.Count });
            }

            return stats;
        }
    }

    public class ResourceStatsVm
    {
        public List<ServerMetadata.ResourceStat> ResourceStats;
    }
}
