namespace Spark.Engine.Interfaces
{
    using System.Collections.Generic;
    using Hl7.Fhir.Model;

    internal interface IResourceValidator
    {
        IEnumerable<OperationOutcome> Validate(Resource resource);
    }
}
