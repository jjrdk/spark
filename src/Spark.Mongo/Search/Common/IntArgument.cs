namespace Spark.Mongo.Search.Common
{
    using Searcher;

    public class IntArgument : Argument
    {
        public override string GroomElement(string value)
        {
            return value != null ? value.Trim() : null;
        }

        public override string ValueToString(ITerm term)
        {
            return term.Operator + term.Value;
        }
        public override bool Validate(string value)
        {
            int i;
            return int.TryParse(value, out i);
        }
    }
}