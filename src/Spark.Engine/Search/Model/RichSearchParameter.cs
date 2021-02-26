using Hl7.Fhir.Model;

namespace Spark.Engine.Search.Model
{
    public class RichSearchParameter: SearchParameter
    {
        public RichSearchParameter(SearchParameter searchParameter)
        {
            this.SearchParameter = searchParameter;
        }

        public readonly SearchParameter SearchParameter;

    }
}
