using Hl7.Fhir.Model;
using Spark.Engine.Core;
using System;
using System.Collections.Generic;

namespace Spark.Engine.Test.Search
{
    using Xunit;

    public class FhirPropertyIndexTests
    {
        private static readonly IFhirModel _fhirModel = new FhirModel();

        [Fact]
        public void TestGetIndex()
        {
            var index = new FhirPropertyIndex(_fhirModel, new List<Type> { typeof(Patient), typeof(Account) });
            Assert.NotNull(index);
        }

        [Fact]
        public void TestExistingPropertyIsFound()
        {
            var index = new FhirPropertyIndex(_fhirModel, new List<Type> { typeof(Patient), typeof(HumanName) });

            var pm = index.findPropertyInfo("Patient", "name");
            Assert.NotNull(pm);

            pm = index.findPropertyInfo("HumanName", "given");
            Assert.NotNull(pm);
        }

        [Fact]
        public void TestTypedNameIsFound()
        {
            var index = new FhirPropertyIndex(_fhirModel, new List<Type> { typeof(ClinicalImpression), typeof(Period) });

            var pm = index.findPropertyInfo("ClinicalImpression", "effectivePeriod");
            Assert.NotNull(pm);
        }

        [Fact]
        public void TestNonExistingPropertyReturnsNull()
        {
            var index = new FhirPropertyIndex(_fhirModel, new List<Type> { typeof(Patient), typeof(Account) });

            var pm = index.findPropertyInfo("TypeNotPresent", "subject");
            Assert.Null(pm);

            pm = index.findPropertyInfo("Patient", "property_not_present");
            Assert.Null(pm);
        }
    }
}
