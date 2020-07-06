using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Structure.Xml
{
    public class XmlDocumentSigner
    {
        private X509Certificate2 certificate;

        public XmlDocumentSigner(X509Certificate2 certificate)
        {
            this.certificate = certificate;
        }

        public XmlDocument Sign(XmlDocument document, Func<XmlDocument, string> funcReferenceUri)
        {
            return Sign(document, document.DocumentElement, funcReferenceUri(document));
        }

        public XmlDocument Sign(XmlDocument document, string nodePath, Func<XmlDocument, string> funcReferenceUri)
        {
            return Sign(document, nodePath, funcReferenceUri(document));
        }

        public XmlDocument Sign(XmlDocument document, string nodePath, string referenceUri = "")
        {
            foreach (var node in document.FindNodesFromPath(nodePath))
            {
                Sign(document, node, referenceUri);
            }

            return document;
        }

        public XmlDocument Sign(XmlDocument document, XmlNode node, string referenceUri = "")
        {
            try
            {
                SignedXml signedXml = new SignedXml(document);
                signedXml.SigningKey = certificate.PrivateKey;
                signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

                Reference reference = new Reference() { Uri = referenceUri, DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1" };                
                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                reference.AddTransform(new XmlDsigC14NTransform());
                signedXml.AddReference(reference);

                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(new KeyInfoX509Data(certificate));
                signedXml.KeyInfo = keyInfo;
                signedXml.ComputeSignature();

                XmlElement xmlDigitalSignature = signedXml.GetXml();
                node.AppendChild(document.ImportNode(xmlDigitalSignature, true));

                return document;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao assinar o arquivo XML.", ex);
            }
        }

        public string GetSignatureValue(XmlDocument document, string nodePath)
        {            
            var node = document.FindFirstNodeFromPath(nodePath);
            return node?.InnerText;
        }
    }
}
