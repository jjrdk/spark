namespace Spark.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class with info about a Fhir Type (Resource or Element).
    /// Works on other types as well, but is not intended for it.
    /// </summary>
    public class FhirTypeInfo
    {
        public string TypeName { get; internal set; }

        public Type FhirType { get; internal set; }

        internal List<FhirPropertyInfo> properties;

        public IEnumerable<FhirPropertyInfo> findPropertyInfos(Predicate<FhirPropertyInfo> propertyPredicate)
        {
            return properties?.Where(pi => propertyPredicate(pi));
        }

        /// <summary>
        /// Find the first property that matches the <paramref name="propertyPredicate"/>.
        /// Properties that are FhirElements are preferred over properties that are not.
        /// </summary>
        /// <param name="propertyPredicate"></param>
        /// <returns>PropertyInfo for property that matches the predicate. Null if none matches.</returns>
        public FhirPropertyInfo findPropertyInfo(Predicate<FhirPropertyInfo> propertyPredicate)
        {
            var allMatches = findPropertyInfos(propertyPredicate);
            IEnumerable<FhirPropertyInfo> preferredMatches;
            if (allMatches != null && allMatches.Count() > 1)
            {
                preferredMatches = allMatches.Where(pi => pi.IsFhirElement);
            }
            else
            {
                preferredMatches = allMatches;
            }
            return preferredMatches?.FirstOrDefault();
        }

        /// <summary>
        /// Find the first property with the name <paramref name="propertyName"/>, or where one of the TypedNames matches <paramref name="propertName"/>.
        /// Properties that are FhirElements are preferred over properties that are not.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>PropertyInofo for property that matches this name.</returns>
        public FhirPropertyInfo findPropertyInfo(string propertyName)
        {
            var result = findPropertyInfo(new Predicate<FhirPropertyInfo>(pi => pi.PropertyName == propertyName));
            if (result == null)
            {
                //try it by typed name
                result = findPropertyInfo(new Predicate<FhirPropertyInfo>(pi => pi.TypedNames.Contains(propertyName)));
            }
            return result;
        }
    }
}