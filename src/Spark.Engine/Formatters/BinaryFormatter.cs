/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Spark.Core;
using Spark.Engine.Core;

namespace Spark.Formatters
{
    using System.Text;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Task = System.Threading.Tasks.Task;

    public class BinaryFhirInputFormatter : FhirInputFormatter
    {
        public BinaryFhirInputFormatter() : base()
        {
            SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(FhirMediaType.OCTET_STREAM_CONTENT_HEADER));
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(Resource);
        }

        /// <inheritdoc />
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
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
    }

    public class BinaryFhirOutputFormatter : FhirOutputFormatter
    {
        public BinaryFhirOutputFormatter() : base()
        {
            SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(FhirMediaType.OCTET_STREAM_CONTENT_HEADER));
        }

        protected override bool CanWriteType(Type type)
        {
            return type == typeof(Binary) || type == typeof(FhirResponse);
        }

        /// <inheritdoc />
        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            var binary = (Binary)context.Object;
            context.ContentType = binary.ContentType;
        }

        /// <inheritdoc />
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var binary = (Binary)context.Object;

            var stream = context.HttpContext.Response.Body;
            await stream.WriteAsync(binary.Data).ConfigureAwait(false);
            await stream.FlushAsync().ConfigureAwait(false);
        }
    }
}