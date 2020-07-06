using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Structure.Xml
{
    public class XmlSchemaValidatorHelper
    {
        public event ValidationEventHandler ValidationEventHandler;

        public bool Validade(string targetNamespace, string schemaFileName, Stream xmlStream)
        {
            return ProcessValidade(schemaFileName, xmlStream: xmlStream);
        }

        public bool Validade(string targetNamespace, string schemaFileName, StringReader stringReader)
        {
            return ProcessValidade(schemaFileName, stringReader: stringReader);
        }

        private bool ProcessValidade(string schemaFileName, Stream xmlStream = null, StringReader stringReader = null)
        {
            int count = 0;
            XmlSchemaSet schemas = new XmlSchemaSet() { XmlResolver = new XmlUrlResolver() };
            schemas.Add(null, schemaFileName);

            XmlReaderSettings settings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
            settings.Schemas.Add(schemas);
            settings.ValidationEventHandler += (s, e) => { count++; };

            if (ValidationEventHandler != null)
            {
                settings.ValidationEventHandler += ValidationEventHandler;
            }

            if (xmlStream != null)
            {
                xmlStream.Position = 0L;
            }

            using (XmlReader reader = xmlStream != null ?
                XmlReader.Create(xmlStream, settings) :
                XmlReader.Create(stringReader, settings))
            {
                while (reader.Read())
                { }
            }

            return count == 0;
        }

        public static bool Validade(string targetNamespace, string schemaFileName, Stream xmlStream, Action<ValidationEventArgs> validationEvent)
        {
            return GetValidator(validationEvent).Validade(targetNamespace, schemaFileName, xmlStream);
        }

        public static bool Validade(string targetNamespace, string schemaFileName, string xml, Action<ValidationEventArgs> validationEvent)
        {
            using (var stringReader = new StringReader(xml))
            {
                return GetValidator(validationEvent).Validade(targetNamespace, schemaFileName, stringReader);
            }
        }

        private static XmlSchemaValidatorHelper GetValidator(Action<ValidationEventArgs> validationEvent)
        {
            var validador = new XmlSchemaValidatorHelper();
            validador.ValidationEventHandler += (s, e) => validationEvent(e);
            return validador;
        }
    }
}
