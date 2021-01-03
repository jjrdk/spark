//using Hl7.Fhir.Model;

namespace Spark.Engine.Service.FhirServiceExtensions
{
    using Hl7.Fhir.Model;

    internal interface ICapabilityStatementService : IFhirServiceExtension
    {
        CapabilityStatement GetSparkCapabilityStatement(string sparkVersion);
    }
}