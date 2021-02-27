﻿// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Core
{
    using System;
    using System.Net.Http.Headers;
    using System.Text;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;

    public static class FhirMediaType
    {
        public const string OCTET_STREAM_CONTENT_HEADER = "application/octet-stream";

        public static string GetContentType(Type type, ResourceFormat format)
        {
            return typeof(Resource).IsAssignableFrom(type) || type == typeof(Resource)
                ? format switch
                {
                    ResourceFormat.Json => ContentType.JSON_CONTENT_HEADER,
                    ResourceFormat.Xml => ContentType.XML_CONTENT_HEADER,
                    _ => ContentType.XML_CONTENT_HEADER
                }
                : "application/octet-stream";
        }

        public static MediaTypeHeaderValue GetMediaTypeHeaderValue(Type type, ResourceFormat format)
        {
            var mediatype = GetContentType(type, format);
            var header = new MediaTypeHeaderValue(mediatype) {CharSet = Encoding.UTF8.WebName};
            return header;
        }
    }
}