using Structure.Helpers;
using System;
using System.IO;
using System.Xml;

namespace Structure.Xml
{
    public class XmlDocumentStream : IDisposable
    {
        private XmlDocument xmlDocument;

        public Stream Stream { get; private set; }
        public XmlDocument XmlDocument
        {
            get
            {
                if (xmlDocument == null && Stream != null)
                {
                    xmlDocument = XmlDocumentHelper.SafeLoad(Stream);
                }

                return xmlDocument;
            }
        }

        public XmlDocumentStream(string xml)
        {
            Stream = StreamHelper.LoadStringToStream(xml);
        }

        public XmlDocumentStream(Stream stream, XmlDocument xmlDocument = null)
        {
            Stream = stream;
            this.xmlDocument = xmlDocument;
        }

        public virtual bool ValidateSchema()
        {
            return XmlDocument != null && XmlDocumentHelper.IsValid(Stream);
        }

        public XmlNodeHandler GetXmlNodeHandler()
        {
            return XmlDocument.GetHandler();
        }

        public void Dispose()
        {
            Stream.Dispose();
        }

        public static XmlDocumentStream FromXmlString(string xml)
        {
            var stream = StreamHelper.LoadStringToStream(xml);
            return new XmlDocumentStream(stream);
        }
    }
}
