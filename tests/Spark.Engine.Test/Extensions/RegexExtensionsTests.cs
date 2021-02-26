using System.Text.RegularExpressions;
using Spark.Engine.Extensions;

namespace Spark.Engine.Test.Extensions
{
    using Xunit;

    public class RegexExtensionsTests
    {
        public static readonly Regex Sut = new Regex(@"[^a]*(?<alpha>a)[^a]*");

        [Fact]
        public void TestReplaceNamedGroupNoSuchGroup()
        {
            var input = @"bababa";
            var result = Sut.ReplaceGroup(input, "blabla", "c");
            Assert.Equal(@"bababa", result);
        }

        [Fact]
        public void TestReplaceNamedGroupNoCaptures()
        {
            var input = @"bbbbbb";
            var result = Sut.ReplaceGroup(input, "alpha", "c");
            Assert.Equal(@"bbbbbb", result);
        }

        [Fact]
        public void TestReplaceNamedGroupSingleCapture()
        {
            var input = @"babbbb";
            var result = Sut.ReplaceGroup(input, "alpha", "c");
            Assert.Equal(@"bcbbbb", result);
        }

        [Fact]
        public void TestReplaceNamedGroupMultipleCaptures()
        {
            var input = @"bababa";
            var result = Sut.ReplaceGroup(input, "alpha", "c");
            Assert.Equal(@"bcbcbc", result);
        }
    }
}
