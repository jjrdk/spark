/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Spark.Engine.Core
{
    public static class FhirMediaType
    {
        public const string OCTET_STREAM_CONTENT_HEADER = "application/octet-stream";

        public static string GetContentType(Type type, ResourceFormat format)
        {
            if (typeof(Resource).IsAssignableFrom(type) || type == typeof(Resource))
            {
                return format switch
                {
                    ResourceFormat.Json => ContentType.JSON_CONTENT_HEADER,
                    ResourceFormat.Xml => ContentType.XML_CONTENT_HEADER,
                    _ => ContentType.XML_CONTENT_HEADER
                };
            }

            return "application/octet-stream";
        }

        public static string GetContentTypeHeaderValue(this HttpRequestMessage request)
        {
            MediaTypeHeaderValue headervalue = request.Content.Headers.ContentType;
            return headervalue?.MediaType;
        }

        public static string GetAcceptHeaderValue(this HttpRequestMessage request)
        {
            HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> headers = request.Headers.Accept;
            return headers.FirstOrDefault()?.MediaType;
        }

        public static MediaTypeHeaderValue GetMediaTypeHeaderValue(Type type, ResourceFormat format)
        {
            string mediatype = FhirMediaType.GetContentType(type, format);
            MediaTypeHeaderValue header = new MediaTypeHeaderValue(mediatype) { CharSet = Encoding.UTF8.WebName };
            return header;
        }
    }
}