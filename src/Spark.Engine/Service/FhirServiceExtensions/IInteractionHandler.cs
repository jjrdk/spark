namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System.Threading.Tasks;
    using Core;

    public interface IInteractionHandler
    {
        Task<FhirResponse> HandleInteraction(Entry interaction);
    }
}
