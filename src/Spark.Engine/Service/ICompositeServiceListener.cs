namespace Spark.Engine.Service
{
    using System.Threading.Tasks;
    using Core;

    public interface ICompositeServiceListener : IServiceListener
    {
        void Add(IServiceListener listener);
        void Clear();
        Task Inform(Entry interaction);
    }
}