using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Structure.Xml
{
    public static class XmlDocumentHelper
    {
        private static readonly Hashtable serializers = new Hashtable();

        public static XmlDocument Load(string fileName)
        {
            var document = new XmlDocument();
            document.Load(fileName);
            return document;
        }

        public static XmlDocument SafeLoad(Stream stream, bool preserveWhitespace = false)
        {
            try
            {
                stream.Position = 0;
                var document = new XmlDocument();
                document.PreserveWhitespace = preserveWhitespace;
                document.Load(stream);
                return document;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static XmlDocument Load(Stream stream)
        {
            stream.Position = 0;
            var document = new XmlDocument();
            document.Load(stream);
            return document;
        }

        public static XmlDocument LoadWithStream(string fileName)
        {
            byte[] bytes = File.ReadAllBytes(fileName);

            using (MemoryStream m = new MemoryStream(bytes))
            {
                var document = new XmlDocument();
                document.Load(m);
                return document;
            }
        }

        public static XmlDocument LoadWithStream(string fileName, out Stream stream)
        {
            byte[] bytes = File.ReadAllBytes(fileName);
            stream = new MemoryStream(bytes);
            var document = new XmlDocument();
            document.Load(stream);
            return document;
        }

        public static XmlDocument LoadXml(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);
            return document;
        }

        public static XmlDocument LoadXmlOutStream(string xml, out Stream stream)
        {
            stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(xml);
            writer.Flush();
            return LoadXml(xml);
        }

        public static XmlDocument LoadXmlWithStream(string xml)
        {
            using (var stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(xml);
                    writer.Flush();
                    return Load(stream);
                }
            }
        }

        public static XmlNodeHandler GetHandler(string xml, string nodePath = null)
        {
            var xmlDocument = LoadXmlWithStream(xml);

            return !string.IsNullOrEmpty(nodePath) ?
                xmlDocument.GetHandlerFromPath(nodePath) :
                xmlDocument.GetHandler();
        }

        public static XmlDocument LoadFromObject<T>(T obj)
        {
            var xmlSerializer = GetSerializer<T>();

            using (var stream = new MemoryStream())
            {
                using (TextReader tr = new StreamReader(stream, Encoding.UTF8))
                {
                    xmlSerializer.Serialize(stream, obj);
                    stream.Position = 0;
                    return Load(stream);
                }
            }
        }

        private static XmlSerializer GetSerializer<T>()
        {
            var serializerType = typeof(T).FullName;
            if(serializers.ContainsKey(serializerType))
            {
                return (XmlSerializer)serializers[serializerType];
            }

            var serializer = new XmlSerializer(typeof(T));
            serializers.Add(serializerType, serializer);
            return serializer;
        }

        public static bool IsValid(Stream stream)
        {
            try
            {
                stream.Position = 0;

                XmlReaderSettings settings = new XmlReaderSettings
                {
                    ConformanceLevel = ConformanceLevel.Fragment,
                    IgnoreWhitespace = true,
                    CheckCharacters = true,
                    IgnoreComments = true
                };

                var reader = XmlReader.Create(stream, settings);

                while (reader.Read())
                { }

                stream.Position = 0;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
