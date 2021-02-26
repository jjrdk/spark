namespace Spark.Engine.Interfaces
{
    using Core;

    public interface IFhirResponseInterceptor
    {
        FhirResponse GetFhirResponse(Entry entry, object input);

        bool CanHandle(object input);
    }
}