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
using System.Net.Http;
using Hl7.Fhir.Model;
using Spark.Engine.Core;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Utility;
using Spark.Formatters;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

[assembly: InternalsVisibleTo("Spark.Engine.Test")]
namespace Spark.Engine.Extensions
{
    using Microsoft.AspNetCore.Http;

    public static class HttpRequestFhirExtensions
    {
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

        //internal static void AcquireHeaders(this HttpResponseMessage response, FhirResponse fhirResponse)
        //{
        //    if (fhirResponse.Key != null)
        //    {
        //        response.Headers.ETag = ETag.Create(fhirResponse.Key.VersionId);

        //        Uri location = fhirResponse.Key.ToUri();
        //        response.Headers.Location = location;

        //        if (response.Content != null)
        //        {
        //            response.Content.Headers.ContentLocation = location;
        //            if (fhirResponse.Resource != null && fhirResponse.Resource.Meta != null)
        //            {
        //                response.Content.Headers.LastModified = fhirResponse.Resource.Meta.LastUpdated;
        //            }
        //        }
        //    }
        //}

        //private static HttpResponseMessage CreateBareFhirResponse(this HttpRequestMessage request, FhirResponse fhir)
        //{
        //    bool includebody = request.PreferRepresentation();

        //    if (fhir.Resource != null)
        //    {
        //        if (includebody)
        //        {
        //            if (fhir.Resource is Binary binary && request.IsRawBinaryRequest(typeof(Binary)))
        //            {
        //                return request.CreateResponse(fhir.StatusCode, binary, new BinaryFhirOutputFormatter(), binary.ContentType);
        //            }

        //            return request.CreateResponse(new FhirResponse(fhir.StatusCode, fhir.Resource));
        //        }

        //        return request.CreateResponse(new FhirResponse(fhir.StatusCode));
        //    }

        //    return request.CreateResponse(new FhirResponse(fhir.StatusCode));
        //}

        //private static HttpResponseMessage CreateResponse(this HttpRequestMessage request, FhirResponse fhir)
        //{
        //    HttpResponseMessage message = request.CreateBareFhirResponse(fhir);
        //    message.AcquireHeaders(fhir);
        //    return message;
        //}

        public static string GetParameter(this HttpRequest request, string key)
        {
            foreach (var currentKey in request.Query.Keys)
            {
                if (currentKey == key) return request.Query[currentKey];
            }
            return null;
        }

        public static List<(string, string)> TupledParameters(this HttpRequest request)
        {
            var list = new List<(string, string)>();

            foreach (var currentKey in request.Query.Keys)
            {
                list.AddRange(request.Query[currentKey].Select(v => (currentKey, v)));
            }
            return list;
        }

        //private static string GetValue(this HttpRequestMessage request, string key)
        //{
        //    if (request.Headers.Count() > 0)
        //    {
        //        if (request.Headers.TryGetValues(key, out IEnumerable<string> values))
        //        {
        //            string value = values.FirstOrDefault();
        //            return value;
        //        }
        //        return null;
        //    }
        //    else return null;
        //}

        //private static bool PreferRepresentation(this HttpRequestMessage request)
        //{
        //    string value = request.GetValue("Prefer");
        //    return (value == "return=representation" || value == null);
        //}

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
                if (!ContentType.XML_CONTENT_HEADERS.Contains(contentType) && !ContentType.JSON_CONTENT_HEADERS.Contains(contentType))
                    resource.Id = id;
            }
        }

        /// <summary>
        /// Returns true if the Accept header matches any of the FHIR supported Xml or Json MIME types, otherwise false.
        /// </summary>
        /// <param name="content">An instance of <see cref="HttpRequestMessage"/>.</param>
        /// <returns>Returns true if the Accept header matches any of the FHIR supported Xml or Json MIME types, otherwise false.</returns>
        private static bool IsAcceptHeaderFhirMediaType(this HttpRequestMessage request)
        {
            string accept = request.GetAcceptHeaderValue();
            return ContentType.XML_CONTENT_HEADERS.Contains(accept)
                || ContentType.JSON_CONTENT_HEADERS.Contains(accept);
        }

        internal static bool IsRawBinaryRequest(this HttpRequestMessage request, Type type)
        {
            if (type == typeof(Binary) || type == typeof(FhirResponse))
            {
                bool isFhirMediaType = false;
                if (request.Method == HttpMethod.Get)
                    isFhirMediaType = request.IsAcceptHeaderFhirMediaType();
                else if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
                    isFhirMediaType = request.Content.IsContentTypeHeaderFhirMediaType();

                var ub = new UriBuilder(request.RequestUri);
                // TODO: KM: Path matching is not optimal should be replaced by a more solid solution.
                return ub.Path.Contains("Binary")
                    && !isFhirMediaType;
            }
            else
                return false;
        }

        internal static bool IsRawBinaryPostOrPutRequest(this HttpRequestMessage request)
        {
            var ub = new UriBuilder(request.RequestUri);
            // TODO: KM: Path matching is not optimal should be replaced by a more solid solution.
            return ub.Path.Contains("Binary")
                && !ub.Path.EndsWith("_search")
                && !request.Content.IsContentTypeHeaderFhirMediaType()
                && (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put);
        }
    }
}