﻿// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Auxiliary
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Cryptography.Xml;
    using System.Xml;

    // This code contains parts of the code found at
    // http://www.wiktorzychla.com/2012/12/interoperable-xml-digital-signatures-c_20.html

    public static class XmlSignatureHelper
    {
        public static bool VerifySignature(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            var doc = new XmlDocument { PreserveWhitespace = true };
            doc.LoadXml(xml);

            // If there's no signature => return that we are "valid"
            var signatureNode = FindSignatureElement(doc);
            if (signatureNode == null)
            {
                return true;
            }

            var signedXml = new SignedXml(doc);
            signedXml.LoadXml((XmlElement)signatureNode);

            return signedXml.CheckSignature();
        }


        private static XmlNode FindSignatureElement(XmlDocument doc)
        {
            var signatureElements = doc.DocumentElement.GetElementsByTagName(
                "Signature",
                "http://www.w3.org/2000/09/xmldsig#");
            if (signatureElements.Count == 1)
            {
                return signatureElements[0];
            }

            return signatureElements.Count == 0
                ? (XmlNode)null
                : throw new InvalidOperationException("Document has multiple xmldsig Signature elements");
        }


        public static bool IsSigned(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            // First, a quick check, before reading the full document
            if (!xml.Contains("Signature"))
            {
                return false;
            }

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return FindSignatureElement(doc) != null;
        }

        public static string Sign(string xml, X509Certificate2 certificate)
        {
            if (xml == null) throw new ArgumentNullException(nameof(xml));
            if (certificate == null) throw new ArgumentNullException(nameof(certificate));
            if (!certificate.HasPrivateKey)
                throw new ArgumentException("Certificate should have a private key", nameof(certificate));

            XmlDocument doc = new XmlDocument { PreserveWhitespace = true };
            doc.LoadXml(xml);

            doc.LoadXml(xml);

            var signedXml = new SignedXml(doc) { SigningKey = certificate.PrivateKey };

            // Attach certificate KeyInfo
            var keyInfoData = new KeyInfoX509Data(certificate);
            var keyInfo = new KeyInfo();
            keyInfo.AddClause(keyInfoData);
            signedXml.KeyInfo = keyInfo;

            // Attach transforms
            var reference = new Reference("");
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(includeComments: false));
            reference.AddTransform(new XmlDsigExcC14NTransform(includeComments: false));
            signedXml.AddReference(reference);

            // Compute signature
            signedXml.ComputeSignature();
            var signatureElement = signedXml.GetXml();

            // Add signature to bundle
            doc.DocumentElement.AppendChild(doc.ImportNode(signatureElement, true));

            return doc.OuterXml;
        }
    }
}
