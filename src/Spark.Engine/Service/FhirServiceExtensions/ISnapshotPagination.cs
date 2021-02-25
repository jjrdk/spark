namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System;
    using System.Threading.Tasks;
    using Hl7.Fhir.Model;
    using Spark.Engine.Core;

    public interface ISnapshotPagination
    {
        Task<Bundle> GetPage(int? index = null, Action<Entry> transformElement = null);
    }
}