using Hl7.Fhir.Model;
using Spark.Engine.Extensions;
using System.Linq;

namespace Spark.Engine.Test.Extensions
{
    using System.Collections.Generic;
    using Xunit;

    public class SearchParameterExtensionsTests
    {
        [Fact]
        public void TestSetPropertyPathWithSinglePath()
        {
            SearchParameter sut = new SearchParameter { Base = new List<ResourceType?> { ResourceType.Appointment } };

            sut.SetPropertyPath(new string[] { "Appointment.participant.actor" });

            Assert.Equal("//participant/actor", sut.Xpath);
        }

        [Fact]
        public void TestSetPropertyPathWithMultiplePath()
        {
            SearchParameter sut = new SearchParameter { Base = new List<ResourceType?> { ResourceType.AuditEvent } };
            sut.SetPropertyPath(new string[] { "AuditEvent.participant.reference", "AuditEvent.object.reference" });

            Assert.Equal("//participant/reference | //object/reference", sut.Xpath);
        }

        [Fact]
        public void TestGetPropertyPathWithSinglePath()
        {
            SearchParameter sut = new SearchParameter { Xpath = "//participant/actor" };

            var paths = sut.GetPropertyPath();
            Assert.Single(paths);
            Assert.Contains("participant.actor", paths);
        }

        [Fact]
        public void TestGetPropertyPathWithMultiplePath()
        {
            SearchParameter sut = new SearchParameter { Xpath = "//participant/reference | //object/reference" };

            var paths = sut.GetPropertyPath();
            Assert.Equal(2, paths.Length);
            Assert.Contains("participant.reference", paths);
            Assert.Contains("object.reference", paths);
        }

        [Fact]
        public void TestSetPropertyPathWithPredicate()
        {
            SearchParameter sut = new SearchParameter { Base = new List<ResourceType?> { ResourceType.Slot } };
            sut.SetPropertyPath(new string[] { "Slot.extension(url=http://foo.com/myextension).valueReference" });

            Assert.Equal("//extension(url=http://foo.com/myextension)/valueReference", sut.Xpath);
        }

        [Fact]
        public void TestGetPropertyPathWithPredicate()
        {
            SearchParameter sut =
                new SearchParameter { Xpath = "//extension(url=http://foo.com/myextension)/valueReference" };

            var paths = sut.GetPropertyPath();
            Assert.Single(paths);
            Assert.Equal(@"extension(url=http://foo.com/myextension).valueReference", paths[0]);
        }

        [Fact]
        public void TestMatchExtension()
        {
            var input = "//extension(url=http://foo.com/myextension)/valueReference";
            var result = SearchParameterExtensions.XpathPattern.Match(input).Value;
            Assert.Equal(input, result);
        }
    }
}
