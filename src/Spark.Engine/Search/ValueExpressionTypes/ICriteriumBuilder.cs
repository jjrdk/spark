namespace Spark.Engine.Search.ValueExpressionTypes
{
    public interface ICriteriumBuilder
    {
        IOperationBuilder And(string paramName);
    }
}