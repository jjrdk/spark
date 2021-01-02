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
    using System.Text;
    using Auxiliary;
    using Core;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Serialization;
    using Microsoft.AspNetCore.Mvc.Formatters;

    public class XmlFhirInputFormatter : TextInputFormatter
    {
        private readonly FhirXmlParser _parser;

        public XmlFhirInputFormatter(FhirXmlParser parser)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));

            foreach (var mediaType in ContentType.XML_CONTENT_HEADERS)
            {
                SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(mediaType));
            }
        }

        /// <inheritdoc />
        public override async Tasks.Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
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
                    return await InputFormatterResult.SuccessAsync(resource).ConfigureAwait(false);
                }

                throw Error.Internal("The type {0} expected by the controller can not be deserialized", context.ModelType.Name);
            }
            catch (FormatException exc)
            {
                throw Error.BadRequest("Body parsing failed: " + exc.Message);
            }
        }
    }
}
