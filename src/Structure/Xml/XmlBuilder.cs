using Structure.Extensions;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Structure.Xml
{
    public class XmlBuilder
    {
        private XmlDocument xmlDocument { get; }
        private XmlNode xmlNode { get; }
        private Dictionary<string, string> namespaces;
        private readonly string defaultPrefix;

        private XmlBuilder(XmlDocument xmlDocument, XmlNode xmlNode, Dictionary<string, string> namespaces, string defaultPrefix = null)
        {
            this.xmlDocument = xmlDocument;
            this.xmlNode = xmlNode;
            this.namespaces = namespaces ?? new Dictionary<string, string>();
            this.defaultPrefix = defaultPrefix;
        }

        private XmlBuilder(XmlDocument xmlDocument, string version, string encoding) : this(xmlDocument, default(XmlNode), null)
        {
            this.xmlDocument = xmlDocument;
            ConfigureDeclaration(version, encoding);
        }

        private void ConfigureDeclaration(string version, string encoding)
        {
            var xmlDeclaration = xmlDocument.CreateXmlDeclaration(version, encoding, null);
            var root = xmlDocument.DocumentElement;
            xmlDocument.InsertBefore(xmlDeclaration, root);
        }

        private XmlNode CreateNode(XmlNodeType type, string name)
        {
            var parts = name.Split(":");
            var prefix = default(string);

            if (parts.Length > 1)
            {
                prefix = parts[0];
                name = parts[1];
            }

            return CreateNode(type, prefix, name);
        }

        private XmlNode CreateNode(XmlNodeType type, string prefix, string name)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                prefix = defaultPrefix;
            }

            var namespaceURI = default(string);

            if (!string.IsNullOrEmpty(prefix) &&
                namespaces.ContainsKey(prefix))
            {
                namespaceURI = namespaces[prefix];
            }

            var newNode = xmlDocument.CreateNode(type, prefix, name, namespaceURI);
            (xmlNode ?? xmlDocument).AppendChild(newNode);
            return newNode;
        }

        public XmlBuilder Element(string name, Action<XmlBuilder> action)
        {
            var node = CreateNode(XmlNodeType.Element, name);
            return CreateBuilder(node, action);
        }

        public XmlBuilder WithNamespace(string prefix, Action<XmlBuilder> action)
        {
            var builder = new XmlBuilder(xmlDocument, xmlNode, namespaces, prefix);
            action.Invoke(builder);
            return builder;
        }

        private XmlBuilder CreateBuilder(XmlNode node, Action<XmlBuilder> action)
        {
            var builder = new XmlBuilder(xmlDocument, node, namespaces, defaultPrefix);
            action?.Invoke(builder);
            return builder;
        }

        public void Text<T>(string name, T value)
        {
            Element(name, c =>
            {
                var newNode = c.xmlDocument.CreateTextNode(Convert.ToString(value));
                c.xmlNode.AppendChild(newNode);
            });
        }

        public void NestedElement(string name, Action<XmlBuilder> action)
        {
            var xmlBuilder = this;

            foreach (var item in name.Split("/"))
            {
                xmlBuilder = xmlBuilder.Element(item, null);
            }

            action?.Invoke(xmlBuilder);
        }

        public XmlBuilder DeclareNamespace(string @namespace)
        {
            return DeclareNamespace(null, @namespace);
        }

        public XmlBuilder DeclareNamespace(string prefix, string @namespace)
        {
            if (!string.IsNullOrEmpty(prefix))
            {
                namespaces.Add(prefix, @namespace);
            }

            xmlDocument.DocumentElement.SetAttribute("xmlns" + (!string.IsNullOrEmpty(prefix) ? ":" + prefix : ""), @namespace);
            return this;
        }

        public XmlDocument ToDocument()
        {
            return xmlDocument;
        }

        public static XmlBuilder BeginDocument()
        {
            return new XmlBuilder(new XmlDocument(), default(XmlNode), null);
        }

        public static XmlBuilder BeginDocument(string version, string encoding)
        {
            return new XmlBuilder(new XmlDocument(), version, encoding);
        }
    }
}
