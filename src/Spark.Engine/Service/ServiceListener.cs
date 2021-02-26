namespace Spark.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Spark.Engine.Core;
    using Spark.Engine.Service;

    public class ServiceListener : ICompositeServiceListener
    {
        private readonly ILocalhost _localhost;
        readonly List<IServiceListener> _listeners;

        public ServiceListener(ILocalhost localhost, IServiceListener[] listeners = null)
        {
            this._localhost = localhost;
            if (listeners != null)
            {
                this._listeners = new List<IServiceListener>(listeners.AsEnumerable());
            }
        }

        public void Add(IServiceListener listener)
        {
            this._listeners.Add(listener);
        }

        public void Clear()
        {
            _listeners.Clear();
        }

        public Task Inform(Entry interaction)
        {
            // todo: what we want is not to send localhost to the listener, but to add the Resource.Base. But that is not an option in the current infrastructure.
            // It would modify interaction.Resource, while
            return Task.WhenAll(
                _listeners.Select(listener => listener.Inform(_localhost.GetAbsoluteUri(interaction.Key), interaction)));
        }

        public Task Inform(Uri location, Entry entry)
        {
            return Task.WhenAll(_listeners.Select(listener => listener.Inform(location, entry)));
        }

        public Task InformAsync(Uri location, Entry interaction)
        {
            return Task.WhenAll(_listeners.Select(listener => listener.Inform(location, interaction)));
        }
    }
}
