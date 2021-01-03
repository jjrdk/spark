namespace Spark.Mongo.Search.Common
{
    using Engine.Core;
    using Hl7.Fhir.Model;

    public class Definition
    {
        public Argument Argument { get; set; }
        public string Resource { get; set; }
        public string ParamName { get; set; }
        public string Description { get; set; }
        public SearchParamType ParamType { get; set; }
        public ElementQuery Query { get; set; }

        public override string ToString()
        {
            return $"{Resource.ToLower()}.{ParamName.ToLower()}->{Query}";
        }
    }
}