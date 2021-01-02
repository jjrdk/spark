namespace Spark.Postgres
{
    using System;
    using System.Threading.Tasks;
    using Engine.Interfaces;
    using Hl7.Fhir.Model;
    using Task = System.Threading.Tasks.Task;

    public class GuidGenerator : IGenerator
    {
        /// <inheritdoc />
        public Task<string> NextResourceId(Resource resource)
        {
            return Task.FromResult(Guid.NewGuid().ToString("N"));
        }

        /// <inheritdoc />
        public Task<string> NextVersionId(string resourceIdentifier)
        {
            return Task.FromResult(Guid.NewGuid().ToString("N"));
        }

        /// <inheritdoc />
        public Task<string> NextVersionId(string resourceType, string resourceIdentifier)
        {
            return Task.FromResult(Guid.NewGuid().ToString("N"));
        }
    }
}
