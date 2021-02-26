/* 
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/ewoutkramer/fhir-net-api/master/LICENSE
 */

namespace Spark.Engine.Search.ValueExpressionTypes
{
    using System;

    public interface IOperationBuilder
    {
        ICriteriumBuilder Eq(decimal number);
        ICriteriumBuilder LessThan();

        IStringModifier Matches(string text);


        ITokenModifier Is(string code);

        IValueBuilder On(string dateTime);
        IValueBuilder On(DateTimeOffset dateTime);
        IValueBuilder Before();
        IValueBuilder After();

        ICriteriumBuilder References(string resource, string id);
        ICriteriumBuilder References(Uri location);
        ICriteriumBuilder References(string location);

        ICriteriumBuilder IsMissing { get; }
    }
}
