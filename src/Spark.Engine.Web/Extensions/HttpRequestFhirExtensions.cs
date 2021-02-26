/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Spark.Engine.Test")]
namespace Spark.Engine.Web.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Core;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Utility;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Headers;
    using Microsoft.Net.Http.Headers;
    using Utility;
    using MediaTypeHeaderValue = Microsoft.Net.Http.Headers.MediaTypeHeaderValue;

    public static class HttpRequestFhirExtensions
    {
        public static HistoryParameters ToHistoryParameters(this HttpRequest request)
        {
            return new HistoryParameters(
                request.GetParameter("_count").ParseIntParameter(),
                request.GetParameter("_since").ParseDateParameter(),
                request.GetParameter("_sort"));
        }

        internal static SummaryType RequestSummary(this HttpRequest request)
        {
            request.Query.TryGetValue("_summary", out var stringValues);
            return GetSummary(stringValues.FirstOrDefault());
        }

        /// <summary>
        /// Transfers the id to the <see cref="Hl7.Fhir.Model.Resource"/>.
        /// </summary>
        /// <param name="request">An instance of <see cref="HttpRequest"/>.</param>
        /// <param name="resource">An instance of <see cref="Hl7.Fhir.Model.Resource"/>.</param>
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
            if (headers.Headers.TryGetValue(FhirHttpHeaders.IF_NONE_EXIST, out var values))
            {
                ifNoneExist = values.FirstOrDefault();
            }
            return ifNoneExist;
        }

        public static DateTimeOffset? IfModifiedSince(this HttpRequest request)
        {
            var success = DateTimeOffset.TryParse(request.Headers[HeaderNames.IfModifiedSince], out var result);
            return success ? result : DateTimeOffset.UnixEpoch;
        }

        public static IEnumerable<string> IfNoneMatch(this HttpRequest request) =>
            request.Headers[HeaderNames.IfNoneMatch].Select(h => EntityTagHeaderValue.Parse(h).Tag.Value);

        public static List<Tuple<string, string>> TupledParameters(this HttpRequest request)
        {
            var list = new List<Tuple<string, string>>();

            foreach (var currentKey in request.Query.Keys)
            {
                list.AddRange(request.Query[currentKey].Select(v => Tuple.Create(currentKey, v)));
            }
            return list;
        }

        private static SummaryType GetSummary(string summary)
        {
            var summaryType = string.IsNullOrWhiteSpace(summary) ? SummaryType.False : EnumUtility.ParseLiteral<SummaryType>(summary, true);

            return summaryType ?? SummaryType.False;
        }

        private static void TransferResourceIdIfRawBinary(string contentType, Resource resource, string id)
        {
            if (!string.IsNullOrEmpty(contentType) && resource is Binary && resource.Id == null && id != null)
            {
                if (!ContentType.XML_CONTENT_HEADERS.Contains(contentType) && !ContentType.JSON_CONTENT_HEADERS.Contains(contentType))
                {
                    resource.Id = id;
                }
            }
        }

        public static string GetAcceptHeaderValue(this HttpRequest request)
        {
            var headers =
                MediaTypeHeaderValue.ParseList(request.Headers[HeaderNames.Accept]);
            return headers.FirstOrDefault()?.MediaType.Value;
        }

        ///// <summary>
        ///// Returns true if the Accept header matches any of the FHIR supported Xml or Json MIME types, otherwise false.
        ///// </summary>
        ///// <param name="content">An instance of <see cref="HttpRequestMessage"/>.</param>
        ///// <returns>Returns true if the Accept header matches any of the FHIR supported Xml or Json MIME types, otherwise false.</returns>
        //private static bool IsAcceptHeaderFhirMediaType(this HttpRequest request)
        //{
        //    string accept = request.GetAcceptHeaderValue();
        //    return ContentType.XML_CONTENT_HEADERS.Contains(accept)
        //        || ContentType.JSON_CONTENT_HEADERS.Contains(accept);
        //}

        //internal static bool IsRawBinaryRequest(this HttpRequest request, Type type)
        //{
        //    if (type == typeof(Binary) || type == typeof(FhirResponse))
        //    {
        //        bool isFhirMediaType = false;
        //        if (request.Method == HttpMethod.Get.Method)
        //        {
        //            isFhirMediaType = request.IsAcceptHeaderFhirMediaType();
        //        }
        //        else if (request.Method == HttpMethod.Post.Method || request.Method == HttpMethod.Put.Method)
        //        {
        //            isFhirMediaType = request.ContentType.IsContentTypeHeaderFhirMediaType();
        //        }

        //        var ub = new UriBuilder(request.RequestUri);
        //        // TODO: KM: Path matching is not optimal should be replaced by a more solid solution.
        //        return ub.Path.Contains("Binary")
        //            && !isFhirMediaType;
        //    }
        //    else
        //        return false;
        //}

        internal static bool IsRawBinaryPostOrPutRequest(this HttpRequest request)
        {
            var pathValue = request.Path.Value;
            if (pathValue == null)
            {
                return false;
            }
            //var ub = new UriBuilder(request.RequestUri);
            // TODO: KM: Path matching is not optimal should be replaced by a more solid solution.
            return pathValue.Contains("Binary")
                && !pathValue.EndsWith("_search")
                && !request.ContentType.IsContentTypeHeaderFhirMediaType()
                && (request.Method == HttpMethod.Post.Method || request.Method == HttpMethod.Put.Method);
        }
    }
}