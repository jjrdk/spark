namespace Spark.Engine.Search.Model
{
    using System.Collections.Generic;

    public class SearchParamTypeToken : SearchParamType
    {
        public override Hl7.Fhir.Model.SearchParamType SupportsType => Hl7.Fhir.Model.SearchParamType.Token;

        protected static List<Modifier> AllowedModifiers = new List<Modifier> { Modifier.NONE, Modifier.MISSING, Modifier.TEXT, Modifier.IN, Modifier.BELOW, Modifier.ABOVE, Modifier.NOT_IN };
        public override bool ModifierIsAllowed(ActualModifier modifier)
        {
            return AllowedModifiers.Contains(modifier.Modifier);
        }
    }
}