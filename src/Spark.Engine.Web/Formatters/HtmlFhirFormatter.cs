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
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Core;
    using Engine.Extensions;
    using Extensions;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Serialization;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;
    using Task = Tasks.Task;

    public class HtmlFhirFormatter : TextOutputFormatter
    {
        private readonly FhirXmlSerializer _serializer;

        public HtmlFhirFormatter(FhirXmlSerializer serializer)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        /// <inheritdoc />
        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            context.ContentType = new MediaTypeHeaderValue("text/html").ToString();
        }

        //public override Tasks.Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        //{
        //    try
        //    {
        //        throw new NotSupportedException(string.Format((string) "Cannot read unsupported type {0} from body", (object?) type.Name));
        //    }
        //    catch (FormatException exc)
        //    {
        //        throw Error.BadRequest("Body parsing failed: " + exc.Message);
        //    }
        //}

        /// <inheritdoc />
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var type = context.ObjectType;
            var value = context.Object;

            var writer = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding);
            await writer.WriteLineAsync("<html>").ConfigureAwait(false);
            await writer.WriteLineAsync("<head>").ConfigureAwait(false);
            await writer.WriteLineAsync("  <link rel=\"icon\" href=\"/Content/Fire.png\"></link>").ConfigureAwait(false);
            await writer.WriteLineAsync("  <link rel=\"icon\" href=\"/Content/css/fhir-html.css\"></link>").ConfigureAwait(false);
            await writer.WriteLineAsync("</head>").ConfigureAwait(false);
            await writer.WriteLineAsync("<body>").ConfigureAwait(false);
            if (type == typeof(Resource) || type == typeof(OperationOutcome))
            {
                if (value is Bundle resource1)
                {
                    if (resource1.SelfLink != null)
                    {
                        await writer.WriteLineAsync($"Searching: {resource1.SelfLink.OriginalString}<br/>").ConfigureAwait(false);

                        var ps = ParseQueryString(resource1.SelfLink);
                        if (ps.AllKeys.Contains(FhirParameter.SORT))
                            await writer.WriteLineAsync($"    Sort by: {ps[FhirParameter.SORT]}<br/>").ConfigureAwait(false);
                        if (ps.AllKeys.Contains(FhirParameter.SUMMARY))
                            await writer.WriteLineAsync("    Summary only<br/>").ConfigureAwait(false);
                        if (ps.AllKeys.Contains(FhirParameter.COUNT))
                            await writer.WriteLineAsync($"    Count: {ps[FhirParameter.COUNT]}<br/>").ConfigureAwait(false);
                        if (ps.AllKeys.Contains(FhirParameter.SNAPSHOT_INDEX))
                            await writer.WriteLineAsync($"    From RowNum: {ps[FhirParameter.SNAPSHOT_INDEX]}<br/>").ConfigureAwait(false);
                        if (ps.AllKeys.Contains(FhirParameter.SINCE))
                            await writer.WriteLineAsync($"    Since: {ps[FhirParameter.SINCE]}<br/>").ConfigureAwait(false);


                        foreach (var item in ps.AllKeys.Where(k => !k.StartsWith("_")))
                        {
                            if (ModelInfo.SearchParameters.Exists(s => s.Name == item))
                            {
                                await writer.WriteLineAsync($"    {item}: {ps[item]}<br/>").ConfigureAwait(false);
                            }
                            else
                            {
                                await writer.WriteLineAsync($"    <i>{item}: {ps[item]} (excluded)</i><br/>").ConfigureAwait(false);
                            }
                        }
                    }

                    if (resource1.FirstLink != null)
                        await writer.WriteLineAsync($"First Link: {resource1.FirstLink.OriginalString}<br/>").ConfigureAwait(false);
                    if (resource1.PreviousLink != null)
                        await writer.WriteLineAsync($"Previous Link: {resource1.PreviousLink.OriginalString}<br/>").ConfigureAwait(false);
                    if (resource1.NextLink != null)
                        await writer.WriteLineAsync($"Next Link: {resource1.NextLink.OriginalString}<br/>").ConfigureAwait(false);
                    if (resource1.LastLink != null)
                        await writer.WriteLineAsync($"Last Link: {resource1.LastLink.OriginalString}<br/>").ConfigureAwait(false);

                    // Write the other Bundle Header data
                    await writer.WriteLineAsync(
                        $"<span style=\"word-wrap: break-word; display:block;\">Type: {resource1.Type}, {resource1.Entry.Count} of {resource1.Total}</span>").ConfigureAwait(false);

                    foreach (var item in resource1.Entry)
                    {
                        await writer.WriteLineAsync("<div class=\"item-tile\">").ConfigureAwait(false);
                        if (item.IsDeleted())
                        {
                            if (item.Request != null)
                            {
                                var id = item.Request.Url;
                                await writer.WriteLineAsync($"<span style=\"word-wrap: break-word; display:block;\">{id}</span>").ConfigureAwait(false);
                            }
                            await writer.WriteLineAsync("<hr/>").ConfigureAwait(false);
                            await writer.WriteLineAsync("<b>DELETED</b><br/>").ConfigureAwait(false);
                        }
                        else if (item.Resource != null)
                        {
                            var key = item.Resource.ExtractKey();
                            var visualurl = key.WithoutBase().ToUriString();
                            var realurl = key.ToUriString() + "?_format=html";

                            await writer.WriteLineAsync(
                                $"<a style=\"word-wrap: break-word; display:block;\" href=\"{realurl}\">{visualurl}</a>").ConfigureAwait(false);
                            if (item.Resource.Meta?.LastUpdated != null)
                                await writer.WriteLineAsync(
                                    $"<i>Modified: {item.Resource.Meta.LastUpdated.Value}</i><br/>").ConfigureAwait(false);
                            await writer.WriteLineAsync("<hr/>").ConfigureAwait(false);

                            if (item.Resource is DomainResource)
                            {
                                if ((item.Resource as DomainResource).Text != null && !string.IsNullOrEmpty((item.Resource as DomainResource).Text.Div))
                                    await writer.WriteAsync((item.Resource as DomainResource).Text.Div).ConfigureAwait(false);
                                else
                                    await writer.WriteLineAsync($"Blank Text: {item.Resource.ExtractKey().ToUriString()}<br/>").ConfigureAwait(false);
                            }
                            else
                            {
                                await writer.WriteLineAsync("This is not a domain resource").ConfigureAwait(false);
                            }

                        }
                        await writer.WriteLineAsync("</div>").ConfigureAwait(false);
                    }
                }
                else
                {
                    var resource = (DomainResource)value;
                    var org = resource.ResourceBase + "/" + resource.TypeName + "/" + resource.Id;
                    await writer.WriteLineAsync($"Retrieved: {org}<hr/>").ConfigureAwait(false);

                    var text = resource.Text?.Div;
                    await writer.WriteAsync(text).ConfigureAwait(false);
                    await writer.WriteLineAsync("<hr/>").ConfigureAwait(false);

                    var summary = context.HttpContext.Request.RequestSummary();

                    var xml = _serializer.SerializeToString(resource, summary);
                    var xmlDoc = new System.Xml.XPath.XPathDocument(new StringReader(xml));

                    // And we also need an output writer
                    TextWriter output = new StringWriter(new StringBuilder());

                    // Now for a little magic
                    // Create XML Reader with style-sheet
                    var stylesheetReader = System.Xml.XmlReader.Create(new StringReader(Resources.Resources.RenderXMLasHTML));

                    var xslTransform = new System.Xml.Xsl.XslCompiledTransform();
                    xslTransform.Load(stylesheetReader);
                    xslTransform.Transform(xmlDoc, null, output);

                    await writer.WriteLineAsync(output.ToString()).ConfigureAwait(false);
                }
            }
            await writer.WriteLineAsync("</body>").ConfigureAwait(false);
            await writer.WriteLineAsync("</html>").ConfigureAwait(false);
            await writer.FlushAsync().ConfigureAwait(false);
        }


        private static NameValueCollection ParseQueryString(Uri searchUri)
        {
            var keysCollection = searchUri.SplitParams();
            var response = new NameValueCollection();
            foreach (var (key, value) in keysCollection)
            {
                response.Add(key, value);
            }

            return response;
        }
    }
}
