namespace Spark.Engine.Search.Model
{
    using System.Collections.Generic;

    public class SearchParamTypeReference : SearchParamType
    {
        public override Hl7.Fhir.Model.SearchParamType SupportsType => Hl7.Fhir.Model.SearchParamType.Reference;

        protected static List<Modifier> AllowedModifiers = new List<Modifier> { Modifier.NONE, Modifier.MISSING, Modifier.TYPE };
        public override bool ModifierIsAllowed(ActualModifier modifier)
        {
            return AllowedModifiers.Contains(modifier.Modifier);
        }
    }
}