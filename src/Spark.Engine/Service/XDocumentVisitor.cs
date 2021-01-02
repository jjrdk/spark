﻿namespace Spark.Engine.Service
{
    using System;
    using System.Xml.Linq;
    using Core;

    public static class XDocumentExtensions
    {
        public static void VisitAttributes(this XDocument document, string tagname, string attrName, Action<XAttribute> action)
        {
            var nodes = document.Descendants(Namespaces.XHtml + tagname).Attributes(attrName);
            foreach (var node in nodes)
            {
                action(node);
            }
        }
    }
}
