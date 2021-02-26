namespace Spark.Mongo.Search.Common
{
    public class ReferenceArgument : Argument
    {
        private string Groom(string value)
        {
            if (value != null)
            {
                //value = Regex.Replace(value, "/(?=[^@])", "/@"); // force include @ after "/", so "patient/10" becomes "patient/@10"
                return value.Trim();
            }

            return null;
        }
        public override string GroomElement(string value)
        {
            return this.Groom(value);

        }

    }
}