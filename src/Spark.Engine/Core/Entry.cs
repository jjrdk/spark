﻿namespace Spark.Engine.Core
{
    using Hl7.Fhir.Model;
    using System;
    using Spark.Engine.Extensions;

    public class Entry
    {
        public IKey Key
        {
            get
            {
                if (Resource != null)
                {
                    return Resource.ExtractKey();
                }
                else
                {
                    return _key;
                }
            }
            set
            {
                if (Resource != null)
                {
                    value.ApplyTo(Resource);
                }
                else
                {
                    _key = value;
                }
            }
        }

        public Resource Resource { get; set; }

        public Bundle.HTTPVerb Method { get; private set; }

        // API: HttpVerb should not be in Bundle.
        public DateTimeOffset? When
        {
            get
            {
                if (Resource is { Meta: { } })
                {
                    return Resource.Meta.LastUpdated;
                }
                else
                {
                    return _when;
                }
            }
            set
            {
                if (Resource != null)
                {
                    Resource.Meta ??= new Meta();
                    Resource.Meta.LastUpdated = value?.TruncateToMillis();
                }
                else
                {
                    _when = value;
                }
            }
        }

        public EntryState State { get; set; }

        private IKey _key = null;
        private DateTimeOffset? _when = null;

        private Entry(Bundle.HTTPVerb method, IKey key, DateTimeOffset? when, Resource resource)
        {
            if (resource != null)
            {
                key.ApplyTo(resource);
            }
            else
            {
                Key = key;
            }

            Resource = resource;
            Method = method;
            When = when ?? DateTimeOffset.Now;
            State = EntryState.Undefined;
        }

        public static Entry Create(Bundle.HTTPVerb method, Resource resource)
        {
            return new(method, null, DateTimeOffset.UtcNow, resource);
        }

        public static Entry Create(Bundle.HTTPVerb method, IKey key, Resource resource)
        {
            return new(method, key, DateTimeOffset.UtcNow, resource);
        }

        public static Entry Create(Bundle.HTTPVerb method, IKey key, DateTimeOffset when)
        {
            return new(method, key, when, null);
        }

        /// <summary>
        ///  Creates a deleted entry
        /// </summary>
        public static Entry Delete(IKey key, DateTimeOffset? when)
        {
            return Create(Bundle.HTTPVerb.DELETE, key, when ?? DateTimeOffset.UtcNow);
        }

        public bool IsDelete
        {
            get { return Method == Bundle.HTTPVerb.DELETE; }
            set
            {
                Method = Bundle.HTTPVerb.DELETE;
                Resource = null;
            }
        }

        public bool IsPresent
        {
            get { return Method != Bundle.HTTPVerb.DELETE; }
        }

        public static Entry Post(IKey key, Resource resource)
        {
            return Create(Bundle.HTTPVerb.POST, key, resource);
        }

        public static Entry Post(Resource resource)
        {
            return Create(Bundle.HTTPVerb.POST, resource);
        }

        public static Entry Put(IKey key, Resource resource)
        {
            return Create(Bundle.HTTPVerb.PUT, key, resource);
        }

        public override string ToString()
        {
            return $"{Method} {Key}";
        }
    }
}
