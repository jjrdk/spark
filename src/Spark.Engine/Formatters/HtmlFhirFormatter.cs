/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System.Text;
using Hl7.Fhir.Rest;
using System.Collections.Specialized;
using Spark.Engine;
using Spark.Engine.Core;

namespace Spark.Formatters
{
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Task = System.Threading.Tasks.Task;

    public class HtmlFhirFormatter : TextOutputFormatter
    {
        private readonly FhirXmlSerializer _serializer;

        public HtmlFhirFormatter(FhirXmlSerializer serializer) : base()
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
        }

        /// <inheritdoc />
        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            context.ContentType = new MediaTypeHeaderValue("text/html").ToString();
        }

        /// <inheritdoc />
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var type = context.ObjectType;
            var value = context.Object;
            StreamWriter writer = new StreamWriter(context.HttpContext.Response.Body, Encoding.UTF8);
            await writer.WriteLineAsync("<html>").ConfigureAwait(false);
            await writer.WriteLineAsync("<head>").ConfigureAwait(false);
            await writer.WriteLineAsync("  <link rel=\"icon\" href=\"/Content/Fire.png\"></link>").ConfigureAwait(false);
            await writer.WriteLineAsync("  <link rel=\"icon\" href=\"/Content/css/fhir-html.css\"></link>").ConfigureAwait(false);
            await writer.WriteLineAsync("</head>").ConfigureAwait(false);
            await writer.WriteLineAsync("<body>").ConfigureAwait(false);
            if (type == typeof(Resource) || type == typeof(OperationOutcome))
            {
                if (value is Bundle)
                {
                    Bundle resource = (Bundle)value;

                    if (resource.SelfLink != null)
                    {
                        await writer.WriteLineAsync($"Searching: {resource.SelfLink.OriginalString}<br/>").ConfigureAwait(false);

                        NameValueCollection ps = resource.SelfLink.ParseQueryString();
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

                    if (resource.FirstLink != null)
                        await writer.WriteLineAsync($"First Link: {resource.FirstLink.OriginalString}<br/>").ConfigureAwait(false);
                    if (resource.PreviousLink != null)
                        await writer.WriteLineAsync($"Previous Link: {resource.PreviousLink.OriginalString}<br/>").ConfigureAwait(false);
                    if (resource.NextLink != null)
                        await writer.WriteLineAsync($"Next Link: {resource.NextLink.OriginalString}<br/>").ConfigureAwait(false);
                    if (resource.LastLink != null)
                        await writer.WriteLineAsync($"Last Link: {resource.LastLink.OriginalString}<br/>").ConfigureAwait(false);

                    // Write the other Bundle Header data
                    await writer.WriteLineAsync(
                        $"<span style=\"word-wrap: break-word; display:block;\">Type: {resource.Type.ToString()}, {resource.Entry.Count} of {resource.Total}</span>").ConfigureAwait(false);

                    foreach (var item in resource.Entry)
                    {
                        await writer.WriteLineAsync("<div class=\"item-tile\">").ConfigureAwait(false);
                        if (item.IsDeleted())
                        {
                            if (item.Request != null)
                            {
                                string id = item.Request.Url;
                                await writer.WriteLineAsync($"<span style=\"word-wrap: break-word; display:block;\">{id}</span>").ConfigureAwait(false);
                            }
                            await writer.WriteLineAsync("<hr/>").ConfigureAwait(false);
                            await writer.WriteLineAsync("<b>DELETED</b><br/>").ConfigureAwait(false);
                        }
                        else if (item.Resource != null)
                        {
                            Key key = item.Resource.ExtractKey();
                            string visualurl = key.WithoutBase().ToUriString();
                            string realurl = key.ToUriString() + "?_format=html";

                            await writer.WriteLineAsync(
                                $"<a style=\"word-wrap: break-word; display:block;\" href=\"{realurl}\">{visualurl}</a>").ConfigureAwait(false);
                            if (item.Resource.Meta != null && item.Resource.Meta.LastUpdated.HasValue)
                                await writer.WriteLineAsync(
                                    $"<i>Modified: {item.Resource.Meta.LastUpdated.Value.ToString()}</i><br/>").ConfigureAwait(false);
                            await writer.WriteLineAsync("<hr/>").ConfigureAwait(false);

                            if (item.Resource is DomainResource)
                            {
                                if ((item.Resource as DomainResource).Text != null && !string.IsNullOrEmpty((item.Resource as DomainResource).Text.Div))
                                    writer.Write((item.Resource as DomainResource).Text.Div);
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
                    DomainResource resource = (DomainResource)value;
                    string org = resource.ResourceBase + "/" + resource.TypeName + "/" + resource.Id;
                    await writer.WriteLineAsync($"Retrieved: {org}<hr/>").ConfigureAwait(false);

                    string text = resource.Text?.Div;
                    writer.Write(text);
                    await writer.WriteLineAsync("<hr/>").ConfigureAwait(false);

                    SummaryType summary = requestMessage.RequestSummary();

                    string xml = _serializer.SerializeToString(resource, summary);
                    System.Xml.XPath.XPathDocument xmlDoc = new System.Xml.XPath.XPathDocument(new StringReader(xml));

                    // And we also need an output writer
                    System.IO.TextWriter output = new System.IO.StringWriter(new System.Text.StringBuilder());

                    // Now for a little magic
                    // Create XML Reader with style-sheet
                    System.Xml.XmlReader stylesheetReader = System.Xml.XmlReader.Create(new StringReader(Resources.RenderXMLasHTML));

                    System.Xml.Xsl.XslCompiledTransform xslTransform = new System.Xml.Xsl.XslCompiledTransform();
                    xslTransform.Load(stylesheetReader);
                    xslTransform.Transform(xmlDoc, null, output);

                    await writer.WriteLineAsync(output.ToString()).ConfigureAwait(false);
                }
            }
            await writer.WriteLineAsync("</body>").ConfigureAwait(false);
            await writer.WriteLineAsync("</html>").ConfigureAwait(false);
            writer.Flush();
        }
    }
}
