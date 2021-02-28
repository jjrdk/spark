// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Web.Formatters
{
    using System;
    using System.Text;
    using System.Xml;
    using Core;
    using Engine.Extensions;
    using Extensions;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Serialization;
    using Microsoft.AspNetCore.Mvc.Formatters;

    public class XmlFhirOutputFormatter : TextOutputFormatter
    {
        public static readonly string[] XmlMediaTypes =
        {
            "application/xml", "application/fhir+xml", "application/xml+fhir", "text/xml", "text/xml+fhir"
        };

        private readonly FhirXmlSerializer _serializer;

        public XmlFhirOutputFormatter(FhirXmlSerializer serializer)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            foreach (var mediaType in ContentType.XML_CONTENT_HEADERS)
            {
                SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(mediaType));
            }
        }

        /// <inheritdoc />
        public override bool CanWriteResult(OutputFormatterCanWriteContext context) =>
            SupportedMediaTypes.Contains(context.ContentType.Value) && base.CanWriteResult(context);

        /// <inheritdoc />
        protected override bool CanWriteType(Type type) => typeof(Resource).IsAssignableFrom(type);

        /// <inheritdoc />
        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            context.HttpContext.Response.ContentType =
                FhirMediaType.GetMediaTypeHeaderValue(context.ObjectType, ResourceFormat.Xml).ToString();
        }

        /// <inheritdoc />
        public override System.Threading.Tasks.Task WriteResponseBodyAsync(
            OutputFormatterWriteContext context,
            Encoding selectedEncoding)
        {
            var bodyWriter = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding);
            XmlWriter writer = new XmlTextWriter(bodyWriter);
            var summary = context.HttpContext.Request.RequestSummary();

            if (context.ObjectType == typeof(OperationOutcome))
            {
                var resource = (Resource) context.Object;
                _serializer.Serialize(resource, writer, summary);
            }
            else if (typeof(Resource).IsAssignableFrom(context.ObjectType))
            {
                var resource = (Resource) context.Object;
                _serializer.Serialize(resource, writer, summary);
            }
            else if (context.ObjectType == typeof(FhirResponse))
            {
                var response = context.Object as FhirResponse;
                if (response.HasBody)
                {
                    _serializer.Serialize(response.Resource, writer, summary);
                }
            }

            writer.Flush();
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}