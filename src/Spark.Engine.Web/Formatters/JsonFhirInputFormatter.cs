/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

namespace Spark.Engine.Web.Formatters
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Serialization;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Resource = Hl7.Fhir.Model.Resource;

    public class JsonFhirInputFormatter : TextInputFormatter
    {
        private readonly FhirJsonParser _parser;

        public JsonFhirInputFormatter(FhirJsonParser parser)
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
}
