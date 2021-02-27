// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Store
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ExtendableWith<T> : IEnumerable<T>
    {
        private readonly Dictionary<Type, T> _extensions = new Dictionary<Type, T>();

        public IEnumerator<T> GetEnumerator() => _extensions.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected void AddExtension<TV>(TV extension)
            where TV : T
        {
            foreach (var interfaceType in extension.GetType().GetInterfaces().Where(i => typeof(T).IsAssignableFrom(i)))
            {
                _extensions[interfaceType] = extension;
            }
        }

        protected TV FindExtension<TV>()
            where TV : T =>
            _extensions.ContainsKey(typeof(TV)) ? (TV) _extensions[typeof(TV)] : default;
    }
}