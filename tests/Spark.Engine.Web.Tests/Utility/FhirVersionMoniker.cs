namespace Spark.Engine.Web.Tests.Utility
{
    using Hl7.Fhir.Utility;

    public enum FhirVersionMoniker
    {
        [EnumLiteral("")]
        None = 0,
        [EnumLiteral(FhirVersionUtility.VERSION_R2)]
        R2 = 2,
        [EnumLiteral(FhirVersionUtility.VERSION_R3)]
        R3 = 3,
        [EnumLiteral(FhirVersionUtility.VERSION_R4)]
        R4 = 4,
        [EnumLiteral(FhirVersionUtility.VERSION_R5)]
        R5 = 5,
    }
}