namespace Spark.Engine.Search.Model
{
    using System.Collections.Generic;

    public class SearchParamTypeUri : SearchParamType
    {
        public override Hl7.Fhir.Model.SearchParamType SupportsType => Hl7.Fhir.Model.SearchParamType.Uri;

        protected static List<Modifier> AllowedModifiers = new List<Modifier> { Modifier.NONE, Modifier.MISSING, Modifier.BELOW, Modifier.ABOVE };
        public override bool ModifierIsAllowed(ActualModifier modifier)
        {
            return AllowedModifiers.Contains(modifier.Modifier);
        }
    }
}