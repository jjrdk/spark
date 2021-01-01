using System;

namespace Spark.Engine.Core
{
    public class HistoryParameters
    {
        public HistoryParameters(int? count, DateTimeOffset? since, string sortBy, string format = null)
        {
            Count = count;
            Since = since;
            SortBy = sortBy;
            Format = format;
        }

        public int? Count { get; }

        public DateTimeOffset? Since { get; }

        public string Format { get; }

        public string SortBy { get; }
    }
}