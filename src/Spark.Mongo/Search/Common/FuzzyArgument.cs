namespace Spark.Mongo.Search.Common
{
    using Utils;

    public class FuzzyArgument : Argument
    {
        public override string GroomElement(string value)
        {
            return Soundex.For(value);
        }
    }
}