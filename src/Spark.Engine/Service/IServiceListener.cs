namespace Spark.Engine.Service
{
    using System;
    using System.Threading.Tasks;
    using Core;

    public interface IServiceListener
    {
        Task Inform(Uri location, Entry interaction);
    }

}
