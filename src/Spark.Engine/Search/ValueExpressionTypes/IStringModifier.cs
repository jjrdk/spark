namespace Spark.Engine.Search.ValueExpressionTypes
{
    public interface IStringModifier : ICriteriumBuilder
    {
        ICriteriumBuilder Exactly { get; }
    }
}