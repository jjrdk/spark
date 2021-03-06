﻿// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Extensions
{
    using System.Threading.Tasks;
    using Core;
    using Hl7.Fhir.Model;
    using Interfaces;

    public static class GeneratorKeyExtensions
    {
        public static async Task<Key> NextHistoryKey(this IGenerator generator, IKey key)
        {
            var historykey = key.Clone();
            historykey.VersionId = await generator.NextVersionId(key.TypeName, key.ResourceId).ConfigureAwait(false);
            return historykey;
        }

        public static async Task<Key> NextKey(this IGenerator generator, Resource resource)
        {
            var resourceid = await generator.NextResourceId(resource).ConfigureAwait(false);
            var key = resource.ExtractKey();
            var versionid = await generator.NextVersionId(key.TypeName, resourceid).ConfigureAwait(false);
            return Key.Create(key.TypeName, resourceid, versionid);
        }

        //public static void AddHistoryKeys(this IGenerator generator, List<Entry> entries)
        //{
        //    // PERF: this needs a performance improvement.
        //    foreach (Entry entry in entries)
        //    {
        //        entry.Key = generator.NextHistoryKey(entry.Key);
        //    }
        //}
    }
}