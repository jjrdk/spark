namespace Spark.Engine.Web.Tests
{
    using System;
    using System.Text;
    using Hl7.Fhir.ElementModel;
    using Hl7.Fhir.Introspection;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Serialization;
    using Hl7.Fhir.Specification;
    using Xunit;
    using Xunit.Abstractions;
    using Task = System.Threading.Tasks.Task;

    public class FhirClientTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly TestFhirServer _server;

        public FhirClientTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _server = new TestFhirServer(outputHelper, "https://localhost:60001");
        }

        [Theory]
        [InlineData(ResourceFormat.Json)]
        [InlineData(ResourceFormat.Xml)]
        public async Task CanCreatePatientWithDifferentFormats(ResourceFormat format)
        {
            var client = new FhirClient(
                "https://localhost:60001/fhir",
                new FhirClientSettings
                {
                    CompressRequestBody = true,
                    ParserSettings = ParserSettings.CreateDefault(),
                    PreferCompressedResponses = true,
                    PreferredFormat = format,
                    UseFormatParameter = false,
                    VerifyFhirVersion = false
                },
                messageHandler: _server.Server.CreateHandler());
            var patient = new Patient
            {
                Active = true,
                Name = { new HumanName { Family = "Tester", Use = HumanName.NameUse.Usual } }
            };

            var result = await client.CreateAsync(patient).ConfigureAwait(false);

            Assert.NotNull(result.Id);
        }
    }
}
