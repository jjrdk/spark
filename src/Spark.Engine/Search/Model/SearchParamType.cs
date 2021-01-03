namespace Spark.Engine.Search.Model
{
    public abstract class SearchParamType
    {
        public abstract Hl7.Fhir.Model.SearchParamType SupportsType
            { get; }

        public abstract bool ModifierIsAllowed(ActualModifier modifier);
    }
}
