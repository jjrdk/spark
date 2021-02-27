// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Web.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using Engine.Extensions;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Serialization;
    using Hl7.Fhir.Utility;

    internal static class FhirFileImport
    {
        private static Resource ParseResource(string data)
        {
            if (SerializationUtil.ProbeIsJson(data))
            {
                // TODO read config to determine if PermissiveParsing should be on 
                var parser = new FhirJsonParser(new ParserSettings {PermissiveParsing = true});
                return parser.Parse<Resource>(data);
            }

            if (SerializationUtil.ProbeIsXml(data))
            {
                // TODO read config to determine if PermissiveParsing should be on 
                var parser = new FhirXmlParser(new ParserSettings {PermissiveParsing = true});
                return parser.Parse<Resource>(data);
            }

            throw new FormatException("Data is neither Json nor Xml");
        }

        public static IEnumerable<Resource> ImportData(string data)
        {
            var resource = ParseResource(data);
            if (resource is Bundle)
            {
                var bundle = resource as Bundle;
                return bundle.GetResources();
            }

            return new[] {resource};
        }

        public static IEnumerable<Resource> ImportFile(string filename)
        {
            var data = File.ReadAllText(filename);
            return ImportData(data);
        }

        public static IEnumerable<string> ExtractZipEntries(this byte[] buffer)
        {
            using (Stream stream = new MemoryStream(buffer))
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                {
                    var reader = new StreamReader(entry.Open());
                    var data = reader.ReadToEnd();
                    yield return data;
                }
            }
        }

        public static IEnumerable<Resource> ExtractResourcesFromZip(this byte[] buffer) =>
            buffer.ExtractZipEntries().SelectMany(ImportData);

        public static IEnumerable<Resource> ImportZip(string filename) =>
            File.ReadAllBytes(filename).ExtractZipEntries().SelectMany(ImportData);

        public static IEnumerable<Resource> ImportEmbeddedZip(string path) =>
            GetPathAsBytes(path).ExtractResourcesFromZip();

        public static byte[] GetPathAsBytes(string path) => File.ReadAllBytes(path);

        public static Bundle ToBundle(this IEnumerable<Resource> resources, Uri @base)
        {
            var bundle = new Bundle();
            foreach (var resource in resources)
            {
                // Make sure that resources without id's are posted.
                if (resource.Id != null)
                {
                    bundle.Append(Bundle.HTTPVerb.PUT, resource);
                }
                else
                {
                    bundle.Append(Bundle.HTTPVerb.POST, resource);
                }
            }

            return bundle;
        }
    }
}