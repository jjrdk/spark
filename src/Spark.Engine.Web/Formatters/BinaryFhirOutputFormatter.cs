namespace Spark.Engine.Web.Formatters
{
    using System;
    using Core;
    using Hl7.Fhir.Model;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;

    public class BinaryFhirOutputFormatter : OutputFormatter
    {
        public BinaryFhirOutputFormatter()
        {
            SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(FhirMediaType.OCTET_STREAM_CONTENT_HEADER));
        }

        /// <inheritdoc />
        protected override bool CanWriteType(Type type)
        {
            return type == typeof(Resource);
        }

        /// <inheritdoc />
        public override async System.Threading.Tasks.Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context.Object is not Binary)
            {
                return;
            }
            var binary = (Binary)context.Object;
            context.HttpContext.Response.Headers[HeaderNames.ContentType] = binary.ContentType;
            var responseBody = context.HttpContext.Response.Body;
            await responseBody.WriteAsync(binary.Data).ConfigureAwait(false);
            await responseBody.FlushAsync().ConfigureAwait(false);
        }
    }
}