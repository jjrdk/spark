using Spark.Engine.Search.Model;

namespace Spark.Engine.Test.Search
{
    using Xunit;

    public class SearchParamTypeTests

    {
        [Fact]
        public void TestModifierIsAllowed()
        {
            var sptString = new SearchParamTypeString();
            var sptReference = new SearchParamTypeReference();

            Assert.True(sptString.ModifierIsAllowed(new ActualModifier("exact")));
            Assert.False(sptReference.ModifierIsAllowed(new ActualModifier("exact")));
            Assert.True(sptReference.ModifierIsAllowed(new ActualModifier("Patient")));
        }
    }
}
