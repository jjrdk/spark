namespace Spark.Engine.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Hl7.Fhir.Model;

    public static class ModelParametersExtensions
    {
        public static IEnumerable<Meta> ExtractMeta(this Parameters parameters)
        {
            foreach (var parameter in parameters.Parameter.Where(p => p.Name == "meta"))
            {
                if (parameter.Value is Meta meta)
                {
                    yield return meta;
                }

            }
        }
    }
}
