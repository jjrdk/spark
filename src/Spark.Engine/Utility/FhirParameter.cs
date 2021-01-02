using System;

namespace Spark.Engine.Utility
{
    public static class FhirParameterParser
    {
        public static DateTimeOffset? ParseDateParameter(this string value)
        {
            return DateTimeOffset.TryParse(value, out var dateTime)
                ? dateTime : (DateTimeOffset?)null;
        }

        public static int? ParseIntParameter(this string value)
        {
            return (int.TryParse(value, out int n)) ? n : default(int?);
        }

        public static bool? ParseBoolParameter(this string value)
        {
            if (value == null) return null;
            try
            {
                //bool b = PrimitiveTypeConverter.ConvertTo<bool>(value);
                return (bool.TryParse(value, out var b)) ? b : default(bool?);
            }
            catch
            {
                return null;
            }
        }
    }
}
