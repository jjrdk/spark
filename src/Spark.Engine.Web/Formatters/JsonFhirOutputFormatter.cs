namespace Spark.Engine.Web.Formatters
{
    using System;
    using System.Text;
    using Core;
    using Extensions;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Serialization;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Newtonsoft.Json;
    using Task = System.Threading.Tasks.Task;

    public class JsonFhirOutputFormatter : TextOutputFormatter
    {
        public static readonly string[] JsonMediaTypes =
        {
            "application/json",
            "application/fhir+json",
            "application/json+fhir",
            "text/json"
        };

        private readonly FhirJsonSerializer _serializer;

        public JsonFhirOutputFormatter(FhirJsonSerializer serializer)
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
            var summary = context.HttpContext.Request.RequestSummary();

            var type = context.ObjectType;
            var value = context.Object;
            if (type == typeof(OperationOutcome))
            {
                var resource = (Resource)value;
                _serializer.Serialize(resource, writer, summary);
            }
            else if (typeof(Resource).IsAssignableFrom(type))
            {
                var resource = (Resource)value;
                _serializer.Serialize(resource, writer, summary);
            }
            else if (typeof(FhirResponse).IsAssignableFrom(type))
            {
                var response = (value as FhirResponse);
                if (response.HasBody)
                {
                    _serializer.Serialize(response.Resource, writer, summary);
                }
            }
        }
    }
}