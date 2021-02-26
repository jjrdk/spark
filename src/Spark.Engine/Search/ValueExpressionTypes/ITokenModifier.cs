namespace Spark.Engine.Search.ValueExpressionTypes
{
    using System;

    public interface ITokenModifier : ICriteriumBuilder
    {
        ICriteriumBuilder In(string ns);
        ICriteriumBuilder In(Uri ns);

    }
}