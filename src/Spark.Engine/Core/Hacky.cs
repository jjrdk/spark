namespace Spark.Engine.Core
{
    using System;
    using Hl7.Fhir.Model;

    public static class Hacky
    {
        // This is a class without context, and is more useful when static. --mh
        // But does this method not already exist in ModelInfo????
        public static ResourceType GetResourceTypeForResourceName(string name)
        {
            return (ResourceType)Enum.Parse(typeof(ResourceType), name, true);
        }

        public static string GetResourceNameForResourceType(ResourceType type)
        {
            return Enum.GetName(typeof(ResourceType), type);
        }

    }
}