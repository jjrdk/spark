namespace Spark.Engine.Search.Model
{
    using System.Collections.Generic;

    public class SearchParamTypeComposite : SearchParamType
    {
        public override Hl7.Fhir.Model.SearchParamType SupportsType
        { get { return Hl7.Fhir.Model.SearchParamType.Composite; } }

        protected static List<Modifier> allowedModifiers = new List<Modifier> { Modifier.NONE };
        public override bool ModifierIsAllowed(ActualModifier modifier)
        {
            return allowedModifiers.Contains(modifier.Modifier);
        }
    }
}