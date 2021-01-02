namespace Spark.Engine.Web.Formatters
{
    using System;
    using System.Text;
    using System.Xml;
    using Core;
    using Extensions;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Serialization;
    using Microsoft.AspNetCore.Mvc.Formatters;

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

        /// <inheritdoc />
        public override System.Threading.Tasks.Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
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
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}