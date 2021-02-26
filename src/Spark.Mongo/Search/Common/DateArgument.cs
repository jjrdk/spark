namespace Spark.Mongo.Search.Common
{
    using System.Text.RegularExpressions;

    public class DateArgument : Argument
    {
        private string Groom(string value)
        {
            if (value != null)
            {
                var s = Regex.Replace(value, @"[T\s:\-]", "");
                var i = s.IndexOf('+');
                if (i > 0)
                {
                    s = s.Remove(i);
                }

                return s;
            }
            else
            {
                return null;
            }
        }
        public override string GroomElement(string value)
        {
            return Groom(value);
        }
    }
}