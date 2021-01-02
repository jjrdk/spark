/* 
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

namespace Spark.Engine.Service
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Auxiliary;
    using Core;
    using Engine;
    using Extensions;
    using Hl7.Fhir.Model;

    /// <summary>
    /// Import can map id's and references  that are local to the Spark Server to absolute id's and references in outgoing Interactions.
    /// </summary>
    internal class Export
    {
        private readonly ILocalhost localhost;
        private readonly List<Entry> entries;
        private readonly ExportSettings exportSettings;

        public Export(ILocalhost localhost, ExportSettings exportSettings)
        {
            this.localhost = localhost;
            this.exportSettings = exportSettings;
            entries = new List<Entry>();
        }

        public void Add(Entry interaction)
        {
            if (interaction != null && interaction.State == EntryState.Undefined)
            {
                entries.Add(interaction);
            }
        }

        public void Add(IEnumerable<Entry> set)
        {
            foreach (var interaction in set)
            {
                Add(interaction);
            }
        }

        public void Externalize()
        {
            ExternalizeKeys();
            ExternalizeReferences();
            ExternalizeState();
        }

        private void ExternalizeState()
        {
            foreach (var entry in this.entries)
            {
                entry.State = EntryState.External;
            }
        }

        private void ExternalizeKeys()
        {
            foreach(var entry in this.entries)
            {
                ExternalizeKey(entry);
            }
        }

        private void ExternalizeReferences()
        {
            foreach(var entry in this.entries)
            {
                if (entry.Resource != null)
                {
                    ExternalizeReferences(entry.Resource);
                }
            }
        }

        private void ExternalizeKey(Entry entry)
        {
            entry.SupplementBase(localhost.DefaultBase);
        }

        private void ExternalizeReferences(Resource resource)
        {
            Visitor action = (element, name) =>
            {
                if (element == null) return;

                if (element is ResourceReference)
                {
                    var reference = (ResourceReference)element;
                    if (reference.Url != null)
                        reference.Url = new Uri(ExternalizeReference(reference.Url.ToString()), UriKind.RelativeOrAbsolute);
                }
                else if (element is FhirUri)
                {
                    var uri = (FhirUri)element;
                    uri.Value = ExternalizeReference(uri.Value);
                    //((FhirUri)element).Value = LocalizeReference(new Uri(((FhirUri)element).Value, UriKind.RelativeOrAbsolute)).ToString();
                }
                else if (element is Narrative)
                {
                    var n = (Narrative)element;
                    n.Div = FixXhtmlDiv(n.Div);
                }

            };

            Type[] types = { typeof(ResourceReference), typeof(FhirUri), typeof(Narrative) };

            Engine.Auxiliary.ResourceVisitor.VisitByType(resource, action, types);
        }

        //Key ExternalizeReference(Key original)
        //{
        //    KeyKind triage = (localhost.GetKeyKind(original));
        //    if (triage == KeyKind.Foreign | triage == KeyKind.Temporary)
        //    {
        //        Key replacement = mapper.TryGet(original);
        //        if (replacement != null)
        //        {
        //            return replacement;
        //        }
        //        else
        //        {
        //            throw new SparkException(HttpStatusCode.Conflict, "This reference does not point to a resource in the server or the current transaction: {0}", original);
        //        }
        //    }
        //    else if (triage == KeyKind.Local)
        //    {
        //        return original.WithoutBase();
        //    }
        //    else
        //    {
        //        return original;
        //    }
        //}

        private string ExternalizeReference(string uristring)
        {
            if (string.IsNullOrWhiteSpace(uristring)) return uristring;

            var uri = new Uri(uristring, UriKind.RelativeOrAbsolute);

            if (!uri.IsAbsoluteUri && exportSettings.ExternalizeFhirUri)
            {
                var absoluteUri = localhost.Absolute(uri);
                if (absoluteUri.Fragment == uri.ToString()) //don't externalize uri's that are just anchor fragments
                {
                    return uristring;
                }
                else
                {
                    return absoluteUri.ToString();
                }
            }
            else
            {
                return uristring;
            }
        }

        private string FixXhtmlDiv(string div)
        {
            try
            {
                var xdoc = XDocument.Parse(div);
                xdoc.VisitAttributes("img", "src", (n) => n.Value = ExternalizeReference(n.Value));
                xdoc.VisitAttributes("a", "href", (n) => n.Value = ExternalizeReference(n.Value));
                return xdoc.ToString();

            }
            catch
            {
                // illegal xml, don't bother, just return the argument
                // todo: should we really allow illegal xml ? /mh
                return div;
            }

        }

        /*
        public void RemoveBodyFromEntries(List<Entry> entries)
        {
            foreach (Entry entry in entries)
            {
                if (entry.IsResource())
                {
                    entry.Resource = null;
                }
            }
        }
        */
    }
}