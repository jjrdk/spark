using Spark.Engine.Core;
using System.Linq;
using Hl7.Fhir.Model;

namespace Spark.Engine.Test.Core
{
    using Xunit;

    public class FhirModelTests
    {
        private static FhirModel _sut;

        public FhirModelTests()
        {
            _sut = new FhirModel();
        }

        [Fact]
        public void TestCompartments()
        {
            var actual = _sut.FindCompartmentInfo(ResourceType.Patient);

            Assert.NotNull(actual);
            Assert.True(actual.ReverseIncludes.Any());
        }
    }
}
