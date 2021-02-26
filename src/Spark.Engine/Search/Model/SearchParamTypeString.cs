namespace Spark.Engine.Search.Model
{
    using System.Collections.Generic;

    public class SearchParamTypeString : SearchParamType
    {
        public override Hl7.Fhir.Model.SearchParamType SupportsType
        { get { return Hl7.Fhir.Model.SearchParamType.String; } }

        protected static List<Modifier> allowedModifiers = new List<Modifier> { Modifier.NONE, Modifier.MISSING, Modifier.EXACT, Modifier.CONTAINS };
        public override bool ModifierIsAllowed(ActualModifier modifier)
        {
            return allowedModifiers.Contains(modifier.Modifier);
        }
    }
}