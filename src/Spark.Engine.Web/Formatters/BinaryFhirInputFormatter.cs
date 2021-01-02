﻿/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using Tasks = System.Threading.Tasks;

namespace Spark.Engine.Web.Formatters
{
    using System;
    using System.IO;
    using System.Linq;
    using Core;
    using Hl7.Fhir.Model;
    using Microsoft.AspNetCore.Mvc.Formatters;

    public class BinaryFhirInputFormatter : InputFormatter
    {
        public BinaryFhirInputFormatter()
        {
            SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(FhirMediaType.OCTET_STREAM_CONTENT_HEADER));
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(Resource);
        }

        /// <inheritdoc />
        public override async Tasks.Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var success = context.HttpContext.Request.Headers.ContainsKey("X-Content-Type");
            if (!success)
            {
                return await InputFormatterResult.FailureAsync().ConfigureAwait(false);
                //throw Error.BadRequest("POST to binary must provide a Content-Type header");
            }

            string contentType = context.HttpContext.Request.Headers["X-Content-Type"].First();
            MemoryStream stream = new MemoryStream();
            await context.HttpContext.Request.Body.CopyToAsync(stream).ConfigureAwait(false);
            Binary binary = new Binary
            {
                Data = stream.ToArray(),
                ContentType = contentType
            };

            return await InputFormatterResult.SuccessAsync(binary).ConfigureAwait(false);
        }

        //public override Tasks.Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, System.Net.TransportContext transportContext)
        //{
        //    Binary binary = (Binary)value;
        //    var stream = new MemoryStream(binary.Data);
        //    content.Headers.ContentType = new MediaTypeHeaderValue(binary.ContentType);
        //    stream.CopyTo(writeStream);
        //    stream.Flush();

        //    return Tasks.Task.CompletedTask;
        //}
    }
}