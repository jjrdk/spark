/*
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
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Xml;
    using Auxiliary;
    using Core;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Serialization;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Task = Tasks.Task;

    public class XmlFhirInputFormatter : TextInputFormatter
    {
        private readonly FhirXmlParser _parser;
        private readonly FhirXmlSerializer _serializer;

        public XmlFhirInputFormatter(FhirXmlParser parser, FhirXmlSerializer serializer) : base()
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            foreach (var mediaType in ContentType.XML_CONTENT_HEADERS)
            {
                SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(mediaType));
            }
        }

        /// <inheritdoc />
        public override Tasks.Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            return ReadRequestBodyAsync(context, Encoding.UTF8);
        }

        public override Tasks.Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            try
            {
                var reader = context.ReaderFactory(context.HttpContext.Request.Body, encoding);
                var body = await reader.ReadToEndAsync().ConfigureAwait(false);

                if (context.ModelType == typeof(Bundle))
                {
                    if (XmlSignatureHelper.IsSigned(body))
                    {
                        if (!XmlSignatureHelper.VerifySignature(body))
                            throw Error.BadRequest("Digital signature in body failed verification");
                    }
                }

                if (typeof(Resource).IsAssignableFrom(context.ModelType))
                {
                    Resource resource = _parser.Parse<Resource>(body);
                    return Tasks.Task.FromResult<object>(resource);
                }

                throw Error.Internal("The type {0} expected by the controller can not be deserialized", context.ModelType.Name);
            }
            catch (FormatException exc)
            {
                throw Error.BadRequest("Body parsing failed: " + exc.Message);
            }
        }
    }

    public class XmlFhirOutputFormatter : TextOutputFormatter
    {
        private readonly FhirXmlParser _parser;
        private readonly FhirXmlSerializer _serializer;

        public XmlFhirOutputFormatter(FhirXmlParser parser, FhirXmlSerializer serializer) : base()
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            foreach (var mediaType in ContentType.XML_CONTENT_HEADERS)
            {
                SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(mediaType));
            }
        }

        /// <inheritdoc />
        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            context.HttpContext.Response.ContentType = FhirMediaType.GetMediaTypeHeaderValue(context.ObjectType, ResourceFormat.Xml).ToString();
        }

        public override Tasks.Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var bodyWriter = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding);
            XmlWriter writer = new XmlTextWriter(bodyWriter);
            SummaryType summary = context.HttpContext.Request.RequestSummary();

            if (context.ObjectType == typeof(OperationOutcome))
            {
                Resource resource = (Resource)context.Object;
                _serializer.Serialize(resource, writer, summary);
            }
            else if (typeof(Resource).IsAssignableFrom(context.ObjectType))
            {
                Resource resource = (Resource)context.Object;
                _serializer.Serialize(resource, writer, summary);
            }
            else if (context.ObjectType == typeof(FhirResponse))
            {
                FhirResponse response = (context.Object as FhirResponse);
                if (response.HasBody)
                    _serializer.Serialize(response.Resource, writer, summary);
            }

            writer.Flush();
            return Tasks.Task.CompletedTask;
        }
    }
}
