using Hl7.Fhir.Model;
using System;

namespace Spark.Engine.Core
{
    using Extensions;

    public class Entry
    {
        public IKey Key
        {
            get => Resource != null ? Resource.ExtractKey() : _key;
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
        public Bundle.HTTPVerb Method { get; set; }
        // API: HttpVerb should not be in Bundle.
        public DateTimeOffset? When
        {
            get => Resource?.Meta != null ? Resource.Meta.LastUpdated : _when;
            set
            {
                if (Resource != null)
                {
                    Resource.Meta ??= new Meta();
                    Resource.Meta.LastUpdated = value;
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

        protected Entry(Bundle.HTTPVerb method, IKey key, DateTimeOffset? when, Resource resource)
        {
            if (resource != null)
            {
                key.ApplyTo(resource);
            }
            else
            {
                this.Key = key;
            }
            this.Resource = resource;
            this.Method = method;
            this.When = when ?? DateTimeOffset.Now;
            this.State = EntryState.Undefined;
        }


        public static Entry Create(Bundle.HTTPVerb method, Resource resource)
        {
            return new Entry(method, null, null, resource);
        }

        public static Entry Create(Bundle.HTTPVerb method, IKey key, Resource resource)
        {
            return new Entry(method, key, null, resource);
        }

        public static Entry Create(Bundle.HTTPVerb method, IKey key, DateTimeOffset when)
        {
            return new Entry(method, key, when, null);
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
            get => Method == Bundle.HTTPVerb.DELETE;
            set
            {
                Method = Bundle.HTTPVerb.DELETE;
                Resource = null;

            }
        }

        public bool IsPresent => Method != Bundle.HTTPVerb.DELETE;

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

        //public static Interaction GET(IKey key)
        //{
        //    return new Interaction(Bundle.HTTPVerb.GET, key, null, null);
        //}

        public override string ToString()
        {
            return $"{this.Method} {this.Key}";
        }
    }

}
