using System.Diagnostics.Tracing;
using System;

namespace Spark.Engine.Logging
{
    [EventSource(Name = "Furore-Spark-Engine")]
    public sealed class SparkEngineEventSource : EventSource
    {
        public class Keywords
        {
            public const EventKeywords SERVICE_METHOD = (EventKeywords)1;
            public const EventKeywords INVALID = (EventKeywords)2;
            public const EventKeywords UNSUPPORTED = (EventKeywords)4;
            public const EventKeywords TRACING = (EventKeywords)8;
        }

        public class Tasks
        {
            public const EventTask SERVICE_METHOD = (EventTask)1;
        }

        private static readonly Lazy<SparkEngineEventSource> _instance = new Lazy<SparkEngineEventSource>(() => new SparkEngineEventSource());

        private SparkEngineEventSource() { }

        public static SparkEngineEventSource Log => _instance.Value;

        [Event(1, Message = "Service call: {0}",
            Level = EventLevel.Verbose, Keywords = Keywords.SERVICE_METHOD)]
        internal void ServiceMethodCalled(string methodName)
        {
            this.WriteEvent(1, methodName);
        }

        [Event(2, Message = "Not supported: {0} in {1}",
         Level = EventLevel.Verbose, Keywords = Keywords.UNSUPPORTED)]
        internal void UnsupportedFeature(string methodName, string feature)
        {
            this.WriteEvent(2, feature, methodName);
        }

        [Event(4, Message = "Invalid Element",
         Level = EventLevel.Verbose, Keywords = Keywords.UNSUPPORTED)]
        internal void InvalidElement(string resourceId, string element, string message)
        {
            this.WriteEvent(4, message, resourceId, element);
        }
    }
}
