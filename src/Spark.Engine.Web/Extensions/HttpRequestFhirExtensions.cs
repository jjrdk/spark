﻿/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using Spark.Engine.Core;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Utility;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Spark.Engine.Test")]

namespace Spark.Engine.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Http.Headers;
    using Utility;
    using Web;
    using Web.Extensions;

    public static class HttpRequestFhirExtensions
    {
        internal static void AcquireHeaders(this HttpResponse response, FhirResponse fhirResponse)
        {
            if (fhirResponse.Key != null)
            {
                response.Headers.Add(HttpHeaderName.ETAG, ETag.Create(fhirResponse.Key.VersionId)?.ToString());

                Uri location = fhirResponse.Key.ToUri();
                response.Headers.Add(HttpHeaderName.LOCATION, location.OriginalString);

                if (response.Body != null)
                {
                    response.Headers.Add(HttpHeaderName.CONTENT_LOCATION, location.OriginalString);
                    if (fhirResponse.Resource is {Meta: { }})
                    {
                        response.Headers.Add(HttpHeaderName.LAST_MODIFIED, fhirResponse.Resource.Meta.LastUpdated.Value.ToString("R"));
                    }
                }
            }
        }

        public static IEnumerable<Tuple<string, string>> TupledParameters(this HttpRequest request)
        {
            return request.Query.Select(x => Tuple.Create(x.Key, x.Value.ToString()));
        }

        public static HistoryParameters ToHistoryParameters(this HttpRequest request) =>
               new(
                   request.GetParameter("_count").ParseIntParameter(),
                   request.GetParameter("_since").ParseDateParameter(),
                   request.GetParameter("_sort"));

        public static ConditionalHeaderParameters ToConditionalHeaderParameters(this HttpRequest request) =>
            new(request.IfNoneMatch(), request.IfModifiedSince());

        internal static string GetRequestUri(this HttpRequest request)
        {
            var httpRequestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>();
            return $"{request.Scheme}://{request.Host}{httpRequestFeature.RawTarget}";
        }

        internal static DateTimeOffset? IfModifiedSince(this HttpRequest request)
        {
            request.Headers.TryGetValue("If-Modified-Since", out var values);
            if (!DateTimeOffset.TryParse(values.FirstOrDefault(), out var modified)) return null;
            return modified;
        }

        internal static IEnumerable<string> IfNoneMatch(this HttpRequest request)
        {
            if (!request.Headers.TryGetValue("If-None-Match", out var values)) return Array.Empty<string>();
            return values.ToArray();
        }

        internal static SummaryType RequestSummary(this HttpRequest request)
        {
            request.Query.TryGetValue("_summary", out var stringValues);
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
            if (request.Headers.TryGetValue("Content-Type", out var value))
            {
                var contentType = value.FirstOrDefault();
                TransferResourceIdIfRawBinary(contentType, resource, id);
            }
        }

        public static string IfNoneExist(this RequestHeaders headers)
        {
            string ifNoneExist = null;
            if (headers.Headers.TryGetValue(FhirHttpHeaders.IfNoneExist, out var values))
            {
                ifNoneExist = values.FirstOrDefault();
            }

            return ifNoneExist;
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