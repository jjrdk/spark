/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using System;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using Hl7.Fhir.Rest;
using Spark.Core;
using Spark.Engine.Core;

namespace Spark.Formatters
{
    using System.Text;
    using Engine.Web.Extensions;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Resource = Hl7.Fhir.Model.Resource;
    using Task = System.Threading.Tasks.Task;

    public class JsonFhirInputFormatter : TextInputFormatter
    {
        private readonly FhirJsonParser _parser;

        public JsonFhirInputFormatter(FhirJsonParser parser) : base()
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));

            foreach (var mediaType in ContentType.JSON_CONTENT_HEADERS)
            {
                SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(mediaType));
            }
        }

        /// <inheritdoc />
        protected override bool CanReadType(Type type)
        {
            return typeof(Resource).IsAssignableFrom(type);
        }

        /// <inheritdoc />
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            try
            {
                using var bodyReader = context.ReaderFactory(context.HttpContext.Request.Body, encoding);
                var body = await bodyReader.ReadToEndAsync().ConfigureAwait(false); // base.ReadBodyFromStream(readStream, content);

                Resource resource = _parser.Parse<Resource>(body);
                return await InputFormatterResult.SuccessAsync(resource).ConfigureAwait(false);
            }
            catch (FormatException exception)
            {
                throw Error.BadRequest("Body parsing failed: " + exception.Message);
            }
        }
    }

    public class JsonFhirOutputFormatter : TextOutputFormatter
    {
        private readonly FhirJsonSerializer _serializer;

        public JsonFhirOutputFormatter(FhirJsonSerializer serializer) : base()
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            foreach (var mediaType in ContentType.JSON_CONTENT_HEADERS)
            {
                SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(mediaType));
            }
        }

        /// <inheritdoc />
        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            context.ContentType = FhirMediaType.GetMediaTypeHeaderValue(context.ObjectType, ResourceFormat.Json).ToString();
        }

        /// <inheritdoc />
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            await using var bodyWriter = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding);
            using JsonWriter writer = new JsonTextWriter(bodyWriter);
            SummaryType summary = context.HttpContext.Request.RequestSummary();

            var type = context.ObjectType;
            var value = context.Object;
            if (type == typeof(OperationOutcome))
            {
                Resource resource = (Resource)value;
                _serializer.Serialize(resource, writer, summary);
            }
            else if (typeof(Resource).IsAssignableFrom(type))
            {
                Resource resource = (Resource)value;
                _serializer.Serialize(resource, writer, summary);
            }
            else if (typeof(FhirResponse).IsAssignableFrom(type))
            {
                FhirResponse response = (value as FhirResponse);
                if (response.HasBody)
                {
                    _serializer.Serialize(response.Resource, writer, summary);
                }
            }
        }
    }
}
