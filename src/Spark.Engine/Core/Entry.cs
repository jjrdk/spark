// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Core
{
    using System;
    using Extensions;
    using Hl7.Fhir.Model;

    public class Entry
    {
        private IKey _key;
        private DateTimeOffset? _when;

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

        public Bundle.HTTPVerb Method { get; }

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

        public bool IsDelete => Method == Bundle.HTTPVerb.DELETE;

        public bool IsPresent => Method != Bundle.HTTPVerb.DELETE;


        public static Entry Create(Bundle.HTTPVerb method, Resource resource) =>
            new Entry(method, null, null, resource);

        public static Entry Create(Bundle.HTTPVerb method, IKey key, Resource resource) =>
            new Entry(method, key, null, resource);

        public static Entry Create(Bundle.HTTPVerb method, IKey key, DateTimeOffset when) =>
            new Entry(method, key, when, null);

        /// <summary>
        ///     Creates a deleted entry
        /// </summary>
        public static Entry Delete(IKey key, DateTimeOffset? when) =>
            Create(Bundle.HTTPVerb.DELETE, key, when ?? DateTimeOffset.UtcNow);

        public static Entry Post(IKey key, Resource resource) => Create(Bundle.HTTPVerb.POST, key, resource);
        
        public static Entry Put(IKey key, Resource resource) => Create(Bundle.HTTPVerb.PUT, key, resource);

        //public static Interaction GET(IKey key)
        //{
        //    return new Interaction(Bundle.HTTPVerb.GET, key, null, null);
        //}

        public override string ToString() => $"{Method} {Key}";
    }
}