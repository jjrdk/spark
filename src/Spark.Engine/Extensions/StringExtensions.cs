namespace Spark.Engine.Extensions
{
    public static class StringExtensions
    {
        public static string FirstUpper(this string input)
        {
            return string.IsNullOrWhiteSpace(input) ? input : string.Concat(input.Substring(0, 1).ToUpperInvariant(), input.Remove(0, 1));
        }
    }
}
