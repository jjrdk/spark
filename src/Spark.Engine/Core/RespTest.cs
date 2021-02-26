using System.Net.Http;

namespace Spark.Engine.Core
{
    // THe response class is an abstraction of the Fhir REST responses
    // This way, it's easier to implement multiple WebApi controllers
    // without having to implement functionality twice.
    // The FhirService always responds with a "Response"

    public class RespTest : HttpResponseMessage
    {

    }
}