using System;
using Spark.Engine.Search.Model;

namespace Spark.Engine.Test.Search
{
    using Xunit;

    public class ReverseIncludeTests
    {
        [Fact]
        public void TestParseValid()
        {
            ReverseInclude sut = ReverseInclude.Parse("Patient.actor");

            Assert.Equal("Patient", sut.ResourceType);
            Assert.Equal("actor", sut.SearchPath);
        }
        [Fact]
        public void TestParseValidLongerPath()
        {
            ReverseInclude sut = ReverseInclude.Parse("Provenance.target.patient");

            Assert.Equal("Provenance", sut.ResourceType);
            Assert.Equal("target.patient", sut.SearchPath);
        }
        [Fact]
        public void TestParseNull()
        {
            Assert.Throws<ArgumentNullException>(() => ReverseInclude.Parse(null));
        }

        [Fact]
        public void TestParseInvalid()
        {
            Assert.Throws<ArgumentException>(() => ReverseInclude.Parse("bla;foo"));
        }
    }
}
