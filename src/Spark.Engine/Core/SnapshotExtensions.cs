namespace Spark.Engine.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Hl7.Fhir.Model;

    public static class SnapshotExtensions 
    {
        public static IEnumerable<string> Keys(this Bundle bundle)
        {
            return bundle.GetResources().Keys();
        }

        public static IEnumerable<string> Keys(this IEnumerable<Resource> resources)
        {
            return resources.Select(e => e.VersionId);
        }

       

    }
}