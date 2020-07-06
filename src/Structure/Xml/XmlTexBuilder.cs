using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Structure.Xml
{
    public class XmlTexBuilder : IDisposable
    {
        private MemoryStream stream;
        private XmlTextWriter writer;

        public XmlTextWriter Writer
        {
            get { return writer; }
            set { writer = value; }
        }

        public MemoryStream Stream
        {
            get { return stream; }
            set { stream = value; }
        }

        public XmlTexBuilder(Encoding encoding)
        {
            Initialize(encoding);
        }

        public XmlTexBuilder()
        {
            Initialize(Encoding.UTF8);
        }

        private void Initialize(Encoding encoding)
        {
            Stream = new MemoryStream();
            Writer = new XmlTextWriter(Stream, encoding);
        }

        public void Dispose()
        {
            Writer.Dispose();
            Stream.Dispose();
        }

        public XmlDocument CreateXmlDocument()
        {
            stream.Position = 0L;
            return XmlDocumentHelper.Load(stream);
        }
    }
}
