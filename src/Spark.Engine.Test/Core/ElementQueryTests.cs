using System;
using Spark.Engine.Core;
using Hl7.Fhir.Model;
using System.Collections.Generic;
using System.Linq;

namespace Spark.Engine.Test.Core
{
    using Xunit;

    public class ElementQueryTests
    {
        [Fact]
        public void TestVisitOnePathZeroMatch()
        {
            ElementQuery sut = new ElementQuery("Patient.name");

            Patient testPatient = new Patient();
            var result = new List<object>() ;

            sut.Visit(testPatient, fd => result.Add(fd));

            Assert.Equal(testPatient.Name.Count, result.Where(ob => ob != null).Count());
        }

        [Fact]
        public void TestVisitOnePathOneMatch()
        {
            ElementQuery sut = new ElementQuery("Patient.name");

            Patient testPatient = new Patient();
            var hn = new HumanName().WithGiven("Sjors").AndFamily("Jansen");
            testPatient.Name = new List<HumanName> { hn };

            var result = new List<object>();

            sut.Visit(testPatient, fd => result.Add(fd));

            Assert.Equal(testPatient.Name.Count, result.Count(ob => ob != null));
            Assert.Contains(hn, result);
        }

        [Fact]
        public void TestVisitOnePathTwoMatches()
        {
            ElementQuery sut = new ElementQuery("Patient.name");

            Patient testPatient = new Patient();
            var hn1 = new HumanName().WithGiven("A").AndFamily("B");
            var hn2 = new HumanName().WithGiven("Y").AndFamily("Z");
            testPatient.Name = new List<HumanName> { hn1, hn2 };

            var result = new List<object>();

            sut.Visit(testPatient, fd => result.Add(fd));

            Assert.Equal(testPatient.Name.Count, result.Where(ob => ob != null).Count());
            Assert.Contains(hn1, result);
            Assert.Contains(hn2, result);
        }
    }
}
