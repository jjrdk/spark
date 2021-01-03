namespace Spark.Engine.Auxiliary
{
    using Hl7.Fhir.Model;

    public delegate void Visitor(Element element, string path);
}