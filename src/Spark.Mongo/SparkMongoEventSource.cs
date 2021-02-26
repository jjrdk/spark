using System.Diagnostics.Tracing;
using System;

namespace Spark.Mongo
{
    [EventSource(Name = "Furore-Spark-Mongo")]
    public sealed class SparkMongoEventSource : EventSource
    {
        public class Keywords
        {
            public const EventKeywords TRACING = (EventKeywords)1;
            public const EventKeywords UNSUPPORTED = (EventKeywords)2;
        }

        //public class Tasks
        //{
        //    public const EventTask ServiceMethod = (EventTask)1;
        //}

        private static readonly Lazy<SparkMongoEventSource> _instance = new Lazy<SparkMongoEventSource>(() => new SparkMongoEventSource());

        private SparkMongoEventSource() { }

        public static SparkMongoEventSource Log => _instance.Value;

        [Event(1, Message = "Method call: {0}",
            Level = EventLevel.Verbose, Keywords = Keywords.TRACING)]
        internal void ServiceMethodCalled(string methodName)
        {
            this.WriteEvent(1, methodName);
        }
    }
}
