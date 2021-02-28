// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;

    public class ServiceListener : ICompositeServiceListener
    {
        private readonly List<IServiceListener> _listeners;
        private readonly ILocalhost _localhost;

        public ServiceListener(ILocalhost localhost, IServiceListener[] listeners = null)
        {
            _localhost = localhost;
            if (listeners != null)
            {
                _listeners = new List<IServiceListener>(listeners.AsEnumerable());
            }
        }

        public void Add(IServiceListener listener)
        {
            _listeners.Add(listener);
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
                _listeners.Select(
                    listener => listener.Inform(_localhost.GetAbsoluteUri(interaction.Key), interaction)));
        }

        public Task Inform(Uri location, Entry interaction)
        {
            return Task.WhenAll(_listeners.Select(listener => listener.Inform(location, interaction)));
        }
    }
}