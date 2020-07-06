using System;
using System.Xml;

namespace Structure.Xml.Extensions
{
    public static class XmlTextWriterExtensions
    {
        public static void WriteElementString(this XmlTextWriter writer, string localName, object value)
        {
            writer.WriteElementString(localName, Convert.ToString(value));
        }
    }
}
