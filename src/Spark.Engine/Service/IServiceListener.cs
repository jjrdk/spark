namespace Spark.Service
{
    using System;
    using System.Threading.Tasks;
    using Spark.Engine.Core;

    public interface IServiceListener
    {
        Task Inform(Uri location, Entry interaction);
    }
}
