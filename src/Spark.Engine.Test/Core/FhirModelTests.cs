using Spark.Engine.Core;
using System.Linq;
using Hl7.Fhir.Model;

namespace Spark.Engine.Test.Core
{
    using Xunit;

    public class FhirModelTests
    {
        private static FhirModel sut;

        public FhirModelTests()
        {
            sut = new FhirModel();
        }

        [Fact]
        public void TestCompartments()
        {
            var actual = sut.FindCompartmentInfo(ResourceType.Patient);

            Assert.NotNull(actual);
            Assert.True(actual.ReverseIncludes.Any());
        }
    }
}
