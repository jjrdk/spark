namespace Spark.Engine.Service.FhirServiceExtensions
{
    using Core;
    using Hl7.Fhir.Model;

    public class CapabilityStatementService : ICapabilityStatementService
    {
        private readonly ILocalhost _localhost;

        public CapabilityStatementService(ILocalhost localhost)
        {
            this._localhost = localhost;
        }

        public CapabilityStatement GetSparkCapabilityStatement(string sparkVersion)
        {
           return CapabilityStatementBuilder.GetSparkCapabilityStatement(sparkVersion, _localhost);
        }

    }
}
