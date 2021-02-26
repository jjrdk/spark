namespace Spark.Engine.Search.Model
{
    using System.Collections.Generic;

    public class SearchParamTypeToken : SearchParamType
    {
        public override Hl7.Fhir.Model.SearchParamType SupportsType
        { get { return Hl7.Fhir.Model.SearchParamType.Token; } }

        protected static List<Modifier> allowedModifiers = new List<Modifier> { Modifier.NONE, Modifier.MISSING, Modifier.TEXT, Modifier.IN, Modifier.BELOW, Modifier.ABOVE, Modifier.NOT_IN };
        public override bool ModifierIsAllowed(ActualModifier modifier)
        {
            return allowedModifiers.Contains(modifier.Modifier);
        }
    }
}