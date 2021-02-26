namespace Spark.Mongo.Search.Common
{
    public class MetaArgument : Argument
    {
        private string _field;
        public MetaArgument(string field)
        {
            this._field = field;
        }
    }
}