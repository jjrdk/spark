namespace Spark.Engine.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Hl7.Fhir.Model;

    public static class ModelParametersExtensions
    {
        public static IEnumerable<Meta> ExtractMeta(this Parameters parameters)
        {
            foreach(var parameter in parameters.Parameter.Where(p => p.Name == "meta"))
            {
                var meta = (parameter.Value as Meta);
                if (meta != null)
                {
                    yield return meta;
                }

            }
        }

        public static IEnumerable<Coding> ExtractTags(this Parameters parameters)
        {
            return parameters.ExtractMeta().SelectMany(m => m.Tag);
        }
    }
}