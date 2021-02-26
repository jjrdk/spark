namespace Spark.Engine.Store
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ExtendableWith<T> : IEnumerable<T>
    {
        private readonly Dictionary<Type, T> _extensions = new Dictionary<Type, T>();

        protected void AddExtension<TV>(TV extension)
            where TV : T
        {
            foreach (var interfaceType in extension.GetType().GetInterfaces().Where(i => typeof(T).IsAssignableFrom(i)))
            {
                _extensions[interfaceType] = extension;
            }
        }

        protected TV FindExtension<TV>()
            where TV : T
        {
            return _extensions.ContainsKey(typeof(TV)) ? (TV) _extensions[typeof(TV)] : default(TV);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _extensions.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
