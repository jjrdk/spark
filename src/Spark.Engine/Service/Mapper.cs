/* 
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

namespace Spark.Engine.Service
{
    using System;
    using System.Collections.Generic;

    public class Mapper<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _mapping = new Dictionary<TKey, TValue>();

        public Mapper() { }

        public void Clear()
        {
            _mapping.Clear();
        }

        public TValue TryGet(TKey key)
        {
            TValue value;
            return _mapping.TryGetValue(key, out value) ? value : default;
        }

        public bool Exists(TKey key)
        {
            foreach(var item in _mapping)
            {
                if (item.Key.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        public TValue Remap(TKey key, TValue value)
        {
            if (Exists(key))
            {
                _mapping[key] = value;
            }
            else
            {
                _mapping.Add(key, value);
            }

            return value;
        }

        public void Merge(Mapper<TKey, TValue> mapper)
        {
            foreach (var keyValuePair in mapper._mapping)
            {
                if (!Exists(keyValuePair.Key))
                {
                    this._mapping.Add(keyValuePair.Key, keyValuePair.Value);
                }
                else if(Exists(keyValuePair.Key) && TryGet(keyValuePair.Key).Equals(keyValuePair.Value) == false)
                {
                    throw new InvalidOperationException("Incompatible mappings");
                }
            }
        }
    }
}
