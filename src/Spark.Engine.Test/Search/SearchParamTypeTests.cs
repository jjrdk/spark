using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spark.Engine.Search.Model;

namespace Spark.Engine.Test.Search
{
    
    public class SearchParamTypeTests

    {
        [Fact]
        public void TestModifierIsAllowed()
        {
            var sptString = new SearchParamTypeString();
            var sptReference = new SearchParamTypeReference();

            Assert.True(sptString.ModifierIsAllowed(new ActualModifier("exact")));
            Assert.IsFalse(sptReference.ModifierIsAllowed(new ActualModifier("exact")));
            Assert.True(sptReference.ModifierIsAllowed(new ActualModifier("Patient")));
        }
    }
}
