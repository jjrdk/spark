using Hl7.Fhir.Model;
using Spark.Engine.Core;
using Spark.Engine.Model;
using Spark.Search;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spark.Engine.Search.Tests
{
    using Xunit;

    public class ElementIndexerTests
    {
        private readonly ElementIndexer sut;
        //private ObservableEventListener eventListener;
        //private EventEntry lastLogEntry;

        //private class LogObserver : IObserver<EventEntry>
        //{
        //    private readonly Action<EventEntry> _resultAction;
        //    public LogObserver(Action<EventEntry> resultAction)
        //    {
        //        _resultAction = resultAction;
        //    }
        //    public void OnCompleted()
        //    {
        //    }

        //    public void OnError(Exception error)
        //    {
        //    }

        //    public void OnNext(EventEntry value)
        //    {
        //        _resultAction(value);
        //    }
        //}

        public ElementIndexerTests()
        {
            var fhirModel = new FhirModel();
            //eventListener = new ObservableEventListener();
            //eventListener.EnableEvents(SparkEngineEventSource.Log, EventLevel.LogAlways,
            //      Keywords.All);
            //eventListener.Subscribe(new LogObserver(result => lastLogEntry = result));
            sut = new ElementIndexer(fhirModel);
        }

        [Fact]
        public void ElementIndexerTest()
        {
            Assert.NotNull(sut);
            Assert.IsType<ElementIndexer>(sut);
        }

        [Fact]
        public void ElementMapTest()
        {
            var input = new Annotation();
            input.Text = new Markdown("Text of the annotation");
            var result = sut.Map(input);

        //    Assert.Equal(2, lastLogEntry.EventId); //EventId 2 is related to Unsupported  features.
        }

        [Fact]
        public void FhirDecimalMapTest()
        {
            var input = new FhirDecimal(1081.54M);
            var result = sut.Map(input);
            Assert.Single(result);
            Assert.IsType<NumberValue>(result.First());
            Assert.Equal(1081.54M, ((NumberValue)result.First()).Value);
        }

        private void CheckPeriod(List<Spark.Search.Expression> result, string start, string end)
        {
            var nrOfComponents = 0;
            if (!string.IsNullOrWhiteSpace(start)) nrOfComponents++;
            if (!string.IsNullOrWhiteSpace(end)) nrOfComponents++;

            Assert.Single(result);
            Assert.IsType<CompositeValue>(result.First());
            var comp = result.First() as CompositeValue;
            Assert.Equal(nrOfComponents, comp.Components.Count());

            var currentComponent = 0;
            if (!string.IsNullOrWhiteSpace(start))
            {
                Assert.IsType<IndexValue>(comp.Components[currentComponent]);
                var ixValue = comp.Components[currentComponent] as IndexValue;
                Assert.Equal("start", ixValue.Name);
                Assert.Single(ixValue.Values);
                Assert.IsType<DateTimeValue>(ixValue.Values[0]);
                var dtValue = ixValue.Values[0] as DateTimeValue;
                Assert.Equal(new DateTimeValue(start).Value, dtValue.Value);
                currentComponent++;
            }

            if (!string.IsNullOrWhiteSpace(end))
            {
                Assert.IsType<IndexValue>(comp.Components[currentComponent]);
                var ixValue = comp.Components[currentComponent] as IndexValue;
                Assert.Equal("end", ixValue.Name);
                Assert.Single(ixValue.Values);
                Assert.IsType<DateTimeValue>(ixValue.Values[0]);
                var dtValue = ixValue.Values[0] as DateTimeValue;
                Assert.Equal(new DateTimeValue(end).Value, dtValue.Value);
            }
        }
        [Fact]
        public void FhirDateTimeMapTest()
        {
            var input = new FhirDateTime(2015, 3, 14);
            var result = sut.Map(input);
            CheckPeriod(result, "2015-03-14T00:00:00+00:00", "2015-03-15T00:00:00+00:00");
        }

        [Fact]
        public void PeriodWithStartAndEndMapTest()
        {
            var input = new Period();
            input.StartElement = new FhirDateTime("2015-02");
            input.EndElement = new FhirDateTime("2015-03");
            var result = sut.Map(input);
            CheckPeriod(result, "2015-02-01T00:00:00+00:00", "2015-04-01T00:00:00+00:00");
        }

        [Fact]
        public void PeriodWithJustStartMapTest()
        {
            var input = new Period();
            input.StartElement = new FhirDateTime("2015-02");
            var result = sut.Map(input);
            CheckPeriod(result, "2015-02-01T00:00:00+00:00", null);
        }

        [Fact]
        public void PeriodWithJustEndMapTest()
        {
            var input = new Period();
            input.EndElement = new FhirDateTime("2015-03");
            var result = sut.Map(input);
            CheckPeriod(result, null, "2015-04-01T00:00:00+00:00");
        }

        [Fact]
        public void CodingMapTest()
        {
            var input = new Coding();
            input.CodeElement = new Code("bla");
            input.SystemElement = new FhirUri("http://bla.com");
            input.DisplayElement = new FhirString("bla display");
            var result = sut.Map(input);

            Assert.Single(result);
            Assert.IsType<CompositeValue>(result[0]);
            var comp = result[0] as CompositeValue;

            CheckCoding(comp, "bla", "http://bla.com", "bla display");
        }

        private static void CheckCoding(CompositeValue comp, string code, string system, string text)
        {
            CheckCodingFlexible(comp, new Dictionary<string, string>() { { "code", code }, { "system", system }, { "text", text } });
        }

        private static void CheckCodingFlexible(CompositeValue comp, Dictionary<string, string> elements)
        {
            var elementsToCheck = elements.Where(e => e.Value != null);
            var nrOfElements = elementsToCheck.Count();
            Assert.Equal(nrOfElements, comp.Components.Count());
            foreach (var c in comp.Components)
            {
                Assert.IsType<IndexValue>(c);
            }

            foreach (var element in elementsToCheck)
            {
                var elementIV = (IndexValue)comp.Components.Where(c => (c as IndexValue).Name == element.Key).FirstOrDefault();
                Assert.NotNull(elementIV);//, $"Expected a component '{element.Key}'");
                Assert.Single(elementIV.Values);//, $"Expected exactly one component '{element.Key}'");
                Assert.IsType<StringValue>(elementIV.Values[0]);//, $"Expected component '{element.Key}' to be of type {nameof(StringValue)}");
                var codeSV = (StringValue)elementIV.Values[0];
                Assert.Equal(element.Value, codeSV.Value);//, $"Expected component '{element.Key}' to have the value '{element.Value}'");
            }

        }

        [Fact]
        public void CodeableConceptMapTest()
        {
            var input = new CodeableConcept();
            input.Text = "bla text";
            input.Coding = new List<Coding>();

            var coding1 = new Coding();
            coding1.CodeElement = new Code("bla");
            coding1.SystemElement = new FhirUri("http://bla.com");
            coding1.DisplayElement = new FhirString("bla display");

            var coding2 = new Coding();
            coding2.CodeElement = new Code("flit");
            coding2.SystemElement = new FhirUri("http://flit.com");
            coding2.DisplayElement = new FhirString("flit display");

            input.Coding.Add(coding1);
            input.Coding.Add(coding2);

            var result = sut.Map(input);

            Assert.Equal(3, result.Count()); //1 with text and 2 with the codings it

            //Check wether CodeableConcept.Text is in the result.
            var textIVs = result.Where(c => c.GetType() == typeof(IndexValue) && (c as IndexValue).Name == "text").ToList();
            Assert.Single(textIVs);
            var textIV = (IndexValue)textIVs.FirstOrDefault();
            Assert.NotNull(textIV);
            Assert.Single(textIV.Values);
            Assert.IsType<StringValue>(textIV.Values[0]);
            Assert.Equal("bla text", (textIV.Values[0] as StringValue).Value);

            //Check wether both codings are in the result.
            var codeIVs = result.Where(c => c.GetType() == typeof(CompositeValue)).ToList();
            Assert.Equal(2, codeIVs.Count());

            var codeIV1 = (CompositeValue)codeIVs[0];
            var codeIV2 = (CompositeValue)codeIVs[1];
            if (((codeIV1.Components[0] as IndexValue).Values[0] as StringValue).Value == "bla")
            {
                CheckCoding(codeIV1, "bla", "http://bla.com", "bla display");
                CheckCoding(codeIV2, "flit", "http://flit.com", "flit display");
            }
            else //apparently the codings are in different order in the result.
            {
                CheckCoding(codeIV2, "bla", "http://bla.com", "bla display");
                CheckCoding(codeIV1, "flit", "http://flit.com", "flit display");
            }
        }

        [Fact]
        public void IdentifierMapTest()
        {
            var input = new Identifier();
            input.SystemElement = new FhirUri("id-system");
            input.ValueElement = new FhirString("id-value");

            var result = sut.Map(input);

            Assert.Single(result);
            Assert.IsType<CompositeValue>(result[0]);
            var comp = (CompositeValue)result[0];

            CheckCoding(comp, code: "id-value", system: "id-system", text: null);
        }

        [Fact]
        public void ContactPointMapTest()
        {
            var input = new ContactPoint();
            input.UseElement = new Code<ContactPoint.ContactPointUse>(ContactPoint.ContactPointUse.Mobile);
            input.ValueElement = new FhirString("cp-value");

            var result = sut.Map(input);

            Assert.Single(result);
            Assert.IsType<CompositeValue>(result[0]);
            var comp = (CompositeValue)result[0];

            var codeIV = (IndexValue)comp.Components.Where(c => (c as IndexValue).Name == "code").FirstOrDefault();
            Assert.NotNull(codeIV); //, "Expected a component 'code'");
            Assert.Single(codeIV.Values);
            Assert.IsType<StringValue>(codeIV.Values[0]);
            var codeSV = (StringValue)codeIV.Values[0];
            Assert.Equal("cp-value", codeSV.Value);

            var useIV = (IndexValue)comp.Components.Where(c => (c as IndexValue).Name == "use").FirstOrDefault();
            Assert.NotNull(codeIV); //, "Expected a component 'use'");
            var useCode = (CompositeValue)useIV.Values.Where(c => (c is CompositeValue)).FirstOrDefault();
            Assert.NotNull(useCode); //, $"Expected a value of type {nameof(CompositeValue)} in the 'use' component");
            CheckCoding(useCode, "mobile", null, null);
        }

        [Fact]
        public void FhirBooleanMapTest()
        {
            var input = new FhirBoolean(false);

            var result = sut.Map(input);

            Assert.Single(result);
            Assert.IsType<CompositeValue>(result[0]);
            var comp = (CompositeValue)result[0];

            CheckCoding(comp, code: "false", system: null, text: null);
        }

        [Fact]
        public void ResourceReferenceMapTest()
        {
            var input = new ResourceReference();
            input.ReferenceElement = new FhirString("OtherType/OtherId");

            var result = sut.Map(input);

            Assert.Single(result);
            Assert.IsType<StringValue>(result[0]);
            var sv = (StringValue)result[0];
            Assert.Equal("OtherType/OtherId", sv.Value);
        }

        [Fact]
        public void AddressMapTest()
        {
            var input = new Address();
            input.City = "Amsterdam";
            input.Country = "Netherlands";
            input.Line = new List<string> { "Bruggebouw", "Bos en lommerplein 280" };
            input.PostalCode = "1055 RW";

            var result = sut.Map(input);

            Assert.Equal(5, result.Count()); //2 line elements + city, country and postalcode.
            foreach (var res in result)
            {
                Assert.IsType<StringValue>(res);
            }
            Assert.Single(result.Where(r => (r as StringValue).Value == "Bruggebouw"));
            Assert.Single(result.Where(r => (r as StringValue).Value == "Bos en lommerplein 280"));
            Assert.Single(result.Where(r => (r as StringValue).Value == "Netherlands"));
            Assert.Single(result.Where(r => (r as StringValue).Value == "Amsterdam"));
            Assert.Single(result.Where(r => (r as StringValue).Value == "1055 RW"));
        }

        [Fact]
        public void HumanNameMapTest()
        {
            var input = new HumanName();
            input.WithGiven("Pietje").AndFamily("Puk");

            var result = sut.Map(input);

            Assert.Equal(2, result.Count()); //2 line elements + city, country and postalcode.
            foreach (var res in result)
            {
                Assert.IsType<StringValue>(res);
            }
            Assert.Single(result.Where(r => (r as StringValue).Value == "Pietje"));
            Assert.Single(result.Where(r => (r as StringValue).Value == "Puk"));
        }

        [Fact]
        public void HumanNameOnlyGivenMapTest()
        {
            var input = new HumanName();
            input.WithGiven("Pietje");

            var result = sut.Map(input);

            Assert.Single(result); //2 line elements + city, country and postalcode.
            foreach (var res in result)
            {
                Assert.IsType<StringValue>(res);
            }
            Assert.Single(result.Where(r => (r as StringValue).Value == "Pietje"));
        }

        public void CheckQuantity(List<Spark.Search.Expression> result, decimal? value, string unit, string system, string decimals)
        {
            var nrOfElements = (value.HasValue ? 1 : 0) + new List<string> { unit, system, decimals }.Count(s => s != null);

            Assert.Single(result);
            Assert.IsType<CompositeValue>(result[0]);
            var comp = (CompositeValue)result[0];

            Assert.Equal(nrOfElements, comp.Components.Count());
            Assert.Equal(nrOfElements, comp.Components.Count(c => c.GetType() == typeof(IndexValue)));

            if (value.HasValue)
            {
                var compValue = (IndexValue)comp.Components.FirstOrDefault(c => (c as IndexValue).Name == "value");
                Assert.NotNull(compValue);
                Assert.Single(compValue.Values);
                Assert.IsType<NumberValue>(compValue.Values[0]);
                var numberValue = (NumberValue)compValue.Values[0];
                Assert.Equal(value.Value, numberValue.Value);
            }

            if (unit != null)
            {
                var compUnit = (IndexValue)comp.Components.Where(c => (c as IndexValue).Name == "unit").FirstOrDefault();
                Assert.NotNull(compUnit);
                Assert.Single(compUnit.Values);
                Assert.IsType<StringValue>(compUnit.Values[0]);
                var stringUnit = (StringValue)compUnit.Values[0];
                Assert.Equal(unit, stringUnit.Value);
            }

            if (system != null)
            {
                var compSystem = (IndexValue)comp.Components.Where(c => (c as IndexValue).Name == "system").FirstOrDefault();
                Assert.NotNull(compSystem);
                Assert.Single(compSystem.Values);
                Assert.IsType<StringValue>(compSystem.Values[0]);
                var stringSystem = (StringValue)compSystem.Values[0];
                Assert.Equal(system, stringSystem.Value);
            }

            if (decimals != null)
            {
                var compCode = (IndexValue)comp.Components.Where(c => (c as IndexValue).Name == "decimals").FirstOrDefault();
                Assert.NotNull(compCode);
                Assert.Single(compCode.Values);
                Assert.IsType<StringValue>(compCode.Values[0]);
                var stringCode = (StringValue)compCode.Values[0];
                Assert.Equal(decimals, stringCode.Value);
            }
        }

        [Fact]
        public void QuantityValueUnitMapTest()
        {
            var input = new Quantity();
            input.Value = 10;
            input.Unit = "km";

            var result = sut.Map(input);

            CheckQuantity(result, value: 10, unit: "km", system: null, decimals: null);
        }

        [Fact]
        public void QuantityValueSystemCodeMapTest()
        {
            var input = new Quantity();
            input.Value = 10;
            input.System = "http://unitsofmeasure.org";
            input.Code = "kg";

            var result = sut.Map(input);

            CheckQuantity(result, value: 10000, unit: "g", system: "http://unitsofmeasure.org", decimals: "gE04x1.0");
        }

        [Fact]
        public void CodeMapTest()
        {
            var input = new Code("bla");

            var result = sut.Map(input);

            Assert.Single(result);
            Assert.IsType<StringValue>(result[0]);

            Assert.Equal("bla", (result[0] as StringValue).Value);
        }

        [Fact]
        public void CodedEnumMapTest()
        {
            var input = new Code<AdministrativeGender>(AdministrativeGender.Male);

            var result = sut.Map(input);

            Assert.Single(result);
            Assert.IsType<CompositeValue>(result[0]);

            CheckCoding(result[0] as CompositeValue, "male", null, null);
        }

        [Fact]
        public void FhirStringMapTest()
        {
            var input = new FhirString("bla");

            var result = sut.Map(input);

            Assert.Single(result);
            Assert.IsType<StringValue>(result[0]);

            Assert.Equal("bla", (result[0] as StringValue).Value);
        }

    }
}