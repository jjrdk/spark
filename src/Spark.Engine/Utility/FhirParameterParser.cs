namespace Spark.Engine.Utility
{
    using System;

    public static class FhirParameterParser
    {
        public static DateTimeOffset? ParseDateParameter(this string value)
        {
            return DateTimeOffset.TryParse(value, out var dateTime)
                ? dateTime : (DateTimeOffset?)null;
        }

        public static int? ParseIntParameter(this string value)
        {
            return int.TryParse(value, out var n) ? n : default(int?);
        }
    }
}
