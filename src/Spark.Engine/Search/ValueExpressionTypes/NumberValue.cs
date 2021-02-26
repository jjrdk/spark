/* 
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/ewoutkramer/fhir-net-api/master/LICENSE
 */

namespace Spark.Engine.Search.ValueExpressionTypes
{
    using Hl7.Fhir.Serialization;

    public class NumberValue : ValueExpression
    {
        public decimal Value { get; }

        public NumberValue(decimal value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return PrimitiveTypeConverter.ConvertTo<string>(Value);
        }

        public static NumberValue Parse(string text)
        {
            return new NumberValue(PrimitiveTypeConverter.ConvertTo<decimal>(text));
        }
    }
}
