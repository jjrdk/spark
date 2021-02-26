namespace Spark.Engine.Search.Model
{
    using System.Collections.Generic;

    public class SearchParamTypeDate : SearchParamType
    {
        public override Hl7.Fhir.Model.SearchParamType SupportsType => Hl7.Fhir.Model.SearchParamType.Date;

        protected static List<Modifier> AllowedModifiers = new List<Modifier> { Modifier.NONE, Modifier.MISSING };
        public override bool ModifierIsAllowed(ActualModifier modifier)
        {
            return AllowedModifiers.Contains(modifier.Modifier);
        }
    }
}