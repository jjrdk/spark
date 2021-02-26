namespace Spark.Mongo.Search.Common
{
    using Searcher;

    public class StringArgument : Argument
    {

        public override string ValueToString(ITerm term)
        {
            return "\"" + term.Value + "\"";
        }

    }
}