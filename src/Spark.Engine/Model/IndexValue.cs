namespace Spark.Engine.Model
{
    using System.Collections.Generic;
    using System.Linq;

    using Search.ValueExpressionTypes;

    public class IndexValue : ValueExpression
    {
        public IndexValue()
        {
            _values = new List<Expression>();
        }

        public IndexValue(string name) : this()
        {
            Name = name;
        }

        public IndexValue(string name, List<Expression> values) : this(name)
        {
            Values = values;
        }

        public IndexValue(string name, params Expression[] values) : this(name)
        {
            Values = values.ToList();
        }

        public string Name { get; set; }

        private readonly List<Expression> _values;
        public List<Expression> Values
        {
            get => _values;
            private set => _values.AddRange(value);
        }
    }
}
