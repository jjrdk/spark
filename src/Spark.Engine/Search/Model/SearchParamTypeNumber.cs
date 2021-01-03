﻿namespace Spark.Engine.Search.Model
{
    using System.Collections.Generic;

    public class SearchParamTypeNumber: SearchParamType
    {
        public override Hl7.Fhir.Model.SearchParamType SupportsType
        { get { return Hl7.Fhir.Model.SearchParamType.Number; } }

        protected static List<Modifier> allowedModifiers = new List<Modifier> { Modifier.NONE, Modifier.MISSING };
        public override bool ModifierIsAllowed(ActualModifier modifier)
        {
            return allowedModifiers.Contains(modifier.Modifier);
        }
    }
}