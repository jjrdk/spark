/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Hl7.Fhir.Model;
using Spark.Engine.Core;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Utility;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Spark.Engine.Test")]

namespace Spark.Engine.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Http.Headers;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Primitives;
    using Service.FhirServiceExtensions;
    using Utility;
    using Web.Extensions;
    using Web.Formatters;

    public static class HttpRequestFhirExtensions
    {
        public static IEnumerable<Tuple<string, string>> TupledParameters(this HttpRequest request)
        {
            return request.Query.Select(x => Tuple.Create(x.Key, x.Value.ToString()));
        }

        public static HistoryParameters ToHistoryParameters(this HttpRequest request) =>
               new(
                   request.GetParameter("_count").ParseIntParameter(),
                   request.GetParameter("_since").ParseDateParameter(),
                   request.GetParameter("_sort"));

        private static string WithoutQuotes(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            else
            {
                return s.Trim('"');
            }
        }

        internal static string GetRequestUri(this HttpRequest request)
        {
            var httpRequestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>();
            return $"{request.Scheme}://{request.Host}{httpRequestFeature.RawTarget}";
        }

        internal static DateTimeOffset? IfModifiedSince(this HttpRequest request)
        {
            request.Headers.TryGetValue("If-Modified-Since", out StringValues values);
            if (!DateTimeOffset.TryParse(values.FirstOrDefault(), out DateTimeOffset modified)) return null;
            return modified;
        }

        internal static IEnumerable<string> IfNoneMatch(this HttpRequest request)
        {
            if (!request.Headers.TryGetValue("If-None-Match", out StringValues values)) return new string[0];
            return values.ToArray();
        }

        public static string IfMatchVersionId(this HttpRequest request)
        {
            if (request.Headers.Count == 0) return null;

            if (!request.Headers.TryGetValue("If-Match", out StringValues value)) return null;
            var tag = value.FirstOrDefault();
            if (tag == null) return null;
            return WithoutQuotes(tag);
        }

        internal static SummaryType RequestSummary(this HttpRequest request)
        {
            request.Query.TryGetValue("_summary", out StringValues stringValues);
            return GetSummary(stringValues.FirstOrDefault());
        }

        /// <summary>
        /// Transfers the id to the <see cref="Resource"/>.
        /// </summary>
        /// <param name="request">An instance of <see cref="HttpRequest"/>.</param>
        /// <param name="resource">An instance of <see cref="Resource"/>.</param>
        /// <param name="id">A <see cref="string"/> containing the id to transfer to Resource.Id.</param>
        public static void TransferResourceIdIfRawBinary(this HttpRequest request, Resource resource, string id)
        {
            if (request.Headers.TryGetValue("Content-Type", out StringValues value))
            {
                string contentType = value.FirstOrDefault();
                TransferResourceIdIfRawBinary(contentType, resource, id);
            }
        }

        public static string IfNoneExist(this RequestHeaders headers)
        {
            string ifNoneExist = null;
            if (headers.Headers.TryGetValue(FhirHttpHeaders.IfNoneExist, out StringValues values))
            {
                ifNoneExist = values.FirstOrDefault();
            }

            return ifNoneExist;
        }

        /// <summary>
        /// Returns true if the Accept header matches any of the FHIR supported Xml or Json MIME types, otherwise false.
        /// </summary>
        private static bool IsAcceptHeaderFhirMediaType(this HttpRequest request)
        {
            var acceptHeader = request.GetTypedHeaders().Accept.FirstOrDefault();
            if (acceptHeader == null || acceptHeader.MediaType == StringSegment.Empty)
                return false;

            string accept = acceptHeader.MediaType.Value;
            return ContentType.XML_CONTENT_HEADERS.Contains(accept)
                   || ContentType.JSON_CONTENT_HEADERS.Contains(accept);
        }

        internal static bool IsRawBinaryRequest(this OutputFormatterCanWriteContext context, Type type)
        {
            if (type == typeof(Binary)
                || (type == typeof(FhirResponse)) && ((FhirResponse)context.Object).Resource is Binary)
            {
                HttpRequest request = context.HttpContext.Request;
                bool isFhirMediaType = false;
                if (request.Method == "GET")
                    isFhirMediaType = request.IsAcceptHeaderFhirMediaType();
                else if (request.Method == "POST" || request.Method == "PUT")
                    isFhirMediaType = HttpRequestExtensions.IsContentTypeHeaderFhirMediaType(request.ContentType);

                var ub = new UriBuilder(request.GetRequestUri());
                // TODO: KM: Path matching is not optimal should be replaced by a more solid solution.
                return ub.Path.Contains("Binary") && !isFhirMediaType;
            }
            else
                return false;
        }

        internal static bool IsRawBinaryPostOrPutRequest(this HttpRequest request)
        {
            var ub = new UriBuilder(request.GetRequestUri());
            // TODO: KM: Path matching is not optimal should be replaced by a more solid solution.
            return ub.Path.Contains("Binary")
                   && !ub.Path.EndsWith("_search")
                   && !HttpRequestExtensions.IsContentTypeHeaderFhirMediaType(request.ContentType)
                   && (request.Method == "POST" || request.Method == "PUT");
        }

        internal static void AcquireHeaders(this HttpResponse response, FhirResponse fhirResponse)
        {
            if (fhirResponse.Key != null)
            {
                response.Headers.Add("ETag", ETag.Create(fhirResponse.Key.VersionId)?.ToString());

                Uri location = fhirResponse.Key.ToUri();
                response.Headers.Add("Location", location.OriginalString);

                if (response.Body != null)
                {
                    response.Headers.Add("Content-Location", location.OriginalString);
                    if (fhirResponse.Resource != null && fhirResponse.Resource.Meta != null)
                    {
                        response.Headers.Add(
                            "Last-Modified",
                            fhirResponse.Resource.Meta.LastUpdated.Value.ToString("R"));
                    }
                }
            }
        }

        private static SummaryType GetSummary(string summary)
        {
            SummaryType? summaryType;
            if (string.IsNullOrWhiteSpace(summary))
                summaryType = SummaryType.False;
            else
                summaryType = EnumUtility.ParseLiteral<SummaryType>(summary, true);

            return summaryType ?? SummaryType.False;
        }

        private static void TransferResourceIdIfRawBinary(string contentType, Resource resource, string id)
        {
            if (!string.IsNullOrEmpty(contentType) && resource is Binary && resource.Id == null && id != null)
            {
                if (!ContentType.XML_CONTENT_HEADERS.Contains(contentType)
                    && !ContentType.JSON_CONTENT_HEADERS.Contains(contentType))
                    resource.Id = id;
            }
        }
    }
}