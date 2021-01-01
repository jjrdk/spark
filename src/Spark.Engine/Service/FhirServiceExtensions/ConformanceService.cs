//using Hl7.Fhir.Model;
//using Spark.Engine.Core;

namespace Spark.Engine.Service.FhirServiceExtensions
{
    using Core;
    using Hl7.Fhir.Model;

    public class CapabilityStatementService : ICapabilityStatementService
    {
        private readonly ILocalhost localhost;

        public CapabilityStatementService(ILocalhost localhost)
        {
            this.localhost = localhost;
        }

        public CapabilityStatement GetSparkCapabilityStatement(string sparkVersion)
        {
           return CapabilityStatementBuilder.GetSparkCapabilityStatement(sparkVersion, localhost);
        }

    }
}
