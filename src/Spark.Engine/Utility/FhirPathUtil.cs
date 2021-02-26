using Hl7.Fhir.Introspection;
using Hl7.Fhir.Model;
using System;
using System.Linq;
using System.Reflection;

namespace Spark.Engine.Utility
{
    internal static class FhirPathUtil
    {
        internal static string ConvertToXPathExpression(string fhirPathExpression)
        {
            const string prefix = "f:";
            const string separator = "/";

            var elements = fhirPathExpression.Split('.');
            var xPathExpression = string.Empty;
            foreach (var element in elements)
            {
                if (string.IsNullOrEmpty(xPathExpression))
                {
                    xPathExpression = $"{prefix}{element}";
                }
                else
                {
                    xPathExpression += $"{separator}{prefix}{element}";
                }
            }

            return xPathExpression;
        }

        internal static string ResolveToFhirPathExpression(Type resourceType, string expression)
        {
            var rootType = resourceType;
            var elements = expression.Split('.');
            var length = elements.Length;
            var fhirPathExpression = string.Empty;
            var currentType = rootType;
            for (var i = 0; length > i; i++)
            {
                var elementAndIndexer = GetElementSeparetedFromIndexer(elements[i]);
                var resolvedElement = ResolveElement(currentType, elementAndIndexer.Item1);

                fhirPathExpression += $"{resolvedElement.Item2}{elementAndIndexer.Item2}.";

                currentType = resolvedElement.Item1;
            };

            return fhirPathExpression.Length == 0 ? fhirPathExpression : $"{rootType.Name}.{fhirPathExpression.TrimEnd('.')}";
        }

        internal static (Type, string) ResolveElement(Type root, string element)
        {
            var pi = root.GetProperty(element);
            if (pi == null)
            {
                return (null, element);
            }

            var fhirElementName = element;
            var fhirElement = pi.GetCustomAttribute<FhirElementAttribute>();
            if (fhirElement != null)
            {
                fhirElementName = fhirElement.Name;
            }

            Type elementType;
            if (pi.PropertyType.IsGenericType)
            {
                elementType = pi.PropertyType.GetGenericArguments().FirstOrDefault();
            }
            else
            {
                elementType = pi.PropertyType.UnderlyingSystemType;
            }

            return (elementType, fhirElementName);
        }

        internal static string GetFhirElementForResource<T>(string element)
            where T : Resource
        {
            var mi = typeof(T).GetMember(element).FirstOrDefault();
            if (mi != null)
            {
                var fhirElement = mi.GetCustomAttribute<FhirElementAttribute>();
                if (fhirElement != null)
                {
                    return fhirElement.Name;
                }
            }

            return element;
        }

        internal static (string, string) GetElementSeparetedFromIndexer(string element)
        {
            var index = element.LastIndexOf("[");
            return index > -1 ? (element.Substring(0, index), element.Substring(index)) : (element, string.Empty);
        }
    }
}
