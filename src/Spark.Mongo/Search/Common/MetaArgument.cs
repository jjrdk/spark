namespace Spark.Mongo.Search.Common
{
    public class MetaArgument : Argument
    {
        private string field;
        public MetaArgument(string field)
        {
            this.field = field;
        }
    }
}