﻿/* 
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using Hl7.Fhir.Model;
using System.Collections.Generic;
using Spark.Engine.Core;

namespace Spark.Engine.Extensions
{
    public static class FhirModelExtensions
    {
        public static void Append(this Bundle bundle, Resource resource)
        {
            bundle.Entry.Add(CreateEntryForResource(resource));
        }

        public static void Append(this Bundle bundle, Bundle.HTTPVerb method, Resource resource)
        {
            var entry = CreateEntryForResource(resource);

            if (entry.Request == null)
            {
                entry.Request = new Bundle.RequestComponent();
            }

            entry.Request.Method = method;
            bundle.Entry.Add(entry);
        }

        private static Bundle.EntryComponent CreateEntryForResource(Resource resource)
        {
            var entry = new Bundle.EntryComponent
            {
                Resource = resource,
                //            entry.FullUrl = resource.ResourceIdentity().ToString();
                FullUrl = resource.ExtractKey().ToUriString()
            };
            return entry;
        }

        public static void Append(this Bundle bundle, IEnumerable<Resource> resources)
        {
            foreach (var resource in resources)
            {
                bundle.Append(resource);
            }
        }

        public static void Append(this Bundle bundle, Bundle.HTTPVerb method, IEnumerable<Resource> resources)
        {
            foreach (var resource in resources)
            {
                bundle.Append(method, resource);
            }
        }

        public static Bundle Append(this Bundle bundle, Entry entry, FhirResponse response = null)
        {
            // API: The api should have a function for this. AddResourceEntry doesn't cut it.
            // Might TransactionBuilder be better suitable?

            Bundle.EntryComponent bundleEntry;
            switch (bundle.Type)
            {
                case Bundle.BundleType.History: bundleEntry = entry.ToTransactionEntry(); break;
                case Bundle.BundleType.Searchset: bundleEntry = entry.TranslateToSparseEntry(); break;
                case Bundle.BundleType.BatchResponse: bundleEntry = entry.TranslateToSparseEntry(response); break;
                case Bundle.BundleType.TransactionResponse: bundleEntry = entry.TranslateToSparseEntry(response); break;
                default: bundleEntry = entry.TranslateToSparseEntry(); break;
            }
            bundle.Entry.Add(bundleEntry);

            return bundle;
        }

        public static Bundle Append(this Bundle bundle, IEnumerable<Entry> entries)
        {
            foreach (var entry in entries)
            {
                // BALLOT: whether to send transactionResponse components... not a very clean solution
                bundle.Append(entry);
            }

            // NB! Total can not be set by counting bundle elements, because total is about the snapshot total
            // bundle.Total = bundle.Entry.Count();

            return bundle;
        }

        public static IList<Entry> GetEntries(this ILocalhost localhost, Bundle bundle)
        {
            var entries = new List<Entry>();
            foreach(var bundleEntry in bundle.Entry)
            {
                var entry = localhost.ToInteraction(bundleEntry);
                entries.Add(entry);
            }
            return entries;
        }

    }

}