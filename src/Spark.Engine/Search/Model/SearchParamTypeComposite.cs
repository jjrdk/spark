namespace Spark.Engine.Search.Model
{
    using System.Collections.Generic;

    public class SearchParamTypeComposite : SearchParamType
    {
        public override Hl7.Fhir.Model.SearchParamType SupportsType => Hl7.Fhir.Model.SearchParamType.Composite;

        protected static List<Modifier> AllowedModifiers = new List<Modifier> { Modifier.NONE };
        public override bool ModifierIsAllowed(ActualModifier modifier)
        {
            return AllowedModifiers.Contains(modifier.Modifier);
        }
    }
}