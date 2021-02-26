namespace Spark.Engine.Search.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hl7.Fhir.Model;
    using Support;

    public class ActualModifier
    {
        public string RawModifier { get; set; }

        public Type ModifierType { get; set; }

        public Modifier Modifier { get; set; }

        public const string MISSINGTRUE = "true";
        public const string MISSINGFALSE = "false";
        public const string MISSING_SEPARATOR = "=";

        private static readonly Dictionary<string, Modifier> _mapping = new Dictionary<string, Modifier>
        { {"exact", Modifier.EXACT }
            , {"partial", Modifier.PARTIAL }
            , {"text", Modifier.TEXT}
            , {"contains", Modifier.CONTAINS}
            , {"anyns", Modifier.ANYNAMESPACE }
            , {"missing", Modifier.MISSING }
            , {"below", Modifier.BELOW }
            , {"above", Modifier.ABOVE }
            , {"in", Modifier.IN }
            , {"not-in", Modifier.NOT_IN }
            , {"", Modifier.NONE } };

        public ActualModifier(string rawModifier)
        {
            RawModifier = rawModifier;
            Missing = TryParseMissing(rawModifier);
            if (Missing.HasValue)
            {
                Modifier = Modifier.MISSING;
                return;
            }
            Modifier = _mapping.FirstOrDefault(m => m.Key.Equals(rawModifier, StringComparison.InvariantCultureIgnoreCase)).Value;

            if (Modifier == Modifier.UNKNOWN)
            {
                ModifierType = TryGetType(rawModifier);
                if (ModifierType != null)
                {
                    Modifier = Modifier.TYPE;
                    return;
                }
            }
        }

        public bool? Missing { get; set; }

        /// <summary>
        /// Catches missing, missing=true and missing=false
        /// </summary>
        /// <param name="rawModifier"></param>
        /// <returns></returns>
        private bool? TryParseMissing(string rawModifier)
        {
            var missing = _mapping.FirstOrDefault(m => m.Value == Modifier.MISSING).Key;
            var parts = rawModifier.Split(new string[] { MISSING_SEPARATOR }, StringSplitOptions.None);
            if (parts[0].Equals(missing, StringComparison.InvariantCultureIgnoreCase))
            {
                if (parts.Length > 1)
                {
                    if (parts[1].Equals(MISSINGTRUE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                    else
                    {
                        return parts[1].Equals(MISSINGFALSE, StringComparison.InvariantCultureIgnoreCase)
                            ? (bool?)false
                            : throw Error.Argument("rawModifier", "For the :missing modifier, only values '{0}' and '{1}' are allowed", MISSINGTRUE, MISSINGFALSE);
                    }
                }
                return true;
            }
            return null;
        }

        private Type TryGetType(string rawModifier)
        {
            return ModelInfo.GetTypeForFhirType(rawModifier);
        }

        public override string ToString()
        {
            var modifierText = _mapping.FirstOrDefault(m => m.Value == Modifier).Key;
            switch (Modifier)
            {
                case Modifier.MISSING:
                {
                    return modifierText + MISSING_SEPARATOR + (Missing.Value ? MISSINGTRUE : MISSINGFALSE);
                }
                case Modifier.TYPE:
                {
                    return ModelInfo.GetFhirTypeNameForType(ModifierType);
                }
                default: return modifierText;
            }
        }

    }
}