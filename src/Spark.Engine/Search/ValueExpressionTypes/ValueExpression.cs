namespace Spark.Engine.Search.ValueExpressionTypes
{
    public abstract class ValueExpression : Expression
    {
        public string ToUnescapedString()
        {
            var value = this;
            if (value is UntypedValue untyped)
            {
                value = untyped.AsStringValue();

                return StringValue.UnescapeString(value.ToString());
            }
            return value.ToString();
        }
    }
}