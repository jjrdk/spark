/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

namespace Spark.Engine.Web.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Engine.Extensions;
    using Hl7.Fhir.Rest;
    using Microsoft.AspNetCore.Http;

    public static class HttpRequestExtensions
    {
        internal static void Replace(this HttpHeaders headers, string header, string value)
        {
            //if (headers.Exists(header))
            headers.Remove(header);
            headers.Add(header, value);
        }

        /// <summary>
        /// Returns true if the Content-Type header matches any of the supported Xml or Json MIME types.
        /// </summary>
        /// <param name="content">An instance of <see cref="HttpContent"/>.</param>
        /// <returns>Returns true if the Content-Type header matches any of the supported Xml or Json MIME types.</returns>
        internal static bool IsContentTypeHeaderFhirMediaType(this HttpContent content)
        {
            return IsContentTypeHeaderFhirMediaType(content.Headers.ContentType?.MediaType);
        }

        public static bool IsContentTypeHeaderFhirMediaType(this string contentType)
        {
            if (string.IsNullOrEmpty(contentType)) return false;
            return ContentType.XML_CONTENT_HEADERS.Contains(contentType)
                || ContentType.JSON_CONTENT_HEADERS.Contains(contentType);
        }

        public static string GetParameter(this HttpRequest request, string key)
        {
            string value = null;
            if (request.Query.ContainsKey(key))
            {
                value = request.Query.FirstOrDefault(p => p.Key == key).Value.FirstOrDefault();
            }
            return value;
        }

        public static SearchParams GetSearchParamsFromBody(this HttpRequest request)
        {
            var list = new List<Tuple<string, string>>();

            foreach (var parameter in request.Form)
            {
                list.Add(new Tuple<string, string>(parameter.Key, parameter.Value));
            }

            return request.GetSearchParams().AddAll(list);
        }

        //public static SearchParams GetSearchParamsFromBody(this HttpRequestMessage request)
        //{
        //    var list = new List<Tuple<string, string>>();
        //    string content = request.Content.ReadAsStringAsync().Result;
        //    string[] parameters = string.IsNullOrEmpty(content) ? null : content.Split('&');
        //    foreach (string parameter in parameters)
        //    {
        //        string[] p = parameter.Split('=');
        //        list.Add(new Tuple<string, string>(p[0], Uri.UnescapeDataString(p[1])));
        //    }

        //    return request.GetSearchParams().AddAll(list);
        //}

        public static SearchParams GetSearchParams(this HttpRequest request)
        {
            var parameters = request.TupledParameters().Where(tp => tp.Item1 != "_format");
            var searchCommand = SearchParams.FromUriParamList(parameters.Select(x => Tuple.Create(x.Item1, x.Item2)));
            return searchCommand;
        }
    }
}