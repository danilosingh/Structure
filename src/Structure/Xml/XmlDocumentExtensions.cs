using Structure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace Structure.Xml
{
    public static class XmlDocumentExtensions
    {
        public static XmlNodeHandler GetHandler(this XmlNode doc, string rootNodeName)
        {
            foreach (XmlNode item in doc.ChildNodes)
            {
                if (item.LocalName == rootNodeName)
                {
                    return new XmlNodeHandler(item);
                }
            }

            return null;
        }

        public static XmlNodeHandler GetHandler(this XmlNode doc, params string[] rootNodeNames)
        {
            if (rootNodeNames.Length == 0)
            {
                return new XmlNodeHandler(doc);
            }

            foreach (XmlNode item in doc.ChildNodes)
            {
                if (rootNodeNames.Contains(item.LocalName))
                {
                    return new XmlNodeHandler(item);
                }
            }

            return null;
        }

        public static XmlNodeHandler GetHandlerFromPath(this XmlNode node, string path)
        {
            string[] pathArray = path.Split('/');

            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.LocalName != pathArray[0])
                {
                    continue;
                }

                if (pathArray.Length == 1)
                {
                    return new XmlNodeHandler(item);
                }

                XmlNodeHandler handler = item.GetHandlerFromPath(path.Substring(pathArray[0].Length + 1));

                if (handler != null)
                {
                    return handler;
                }
            }

            return null;
        }

        public static IList<XmlNodeHandler> GetHandlersFromPath(this XmlNode node, string path)
        {
            List<XmlNodeHandler> handlers = new List<XmlNodeHandler>();

            string[] pathArray = path.Split('/');

            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.LocalName != pathArray[0])
                {
                    continue;
                }

                if (pathArray.Length == 1)
                {
                    handlers.Add(new XmlNodeHandler(item));
                }
                else
                {
                    handlers.AddRange(item.GetHandlersFromPath(path.Substring(pathArray[0].Length + 1)));
                }
            }

            return handlers;
        }

        public static XmlNodeHandler GetHandler(this XmlNode doc)
        {
            return new XmlNodeHandler(doc);
        }

        public static void ForFirstNode(this XmlNode doc, string nodeName, Action<XmlNodeHandler> action)
        {
            new XmlNodeHandler(doc).ForFirstNode(nodeName, action);
        }

        public static void ForFirstNodeFromPath(this XmlNode doc, string nodeName, Action<XmlNodeHandler> action)
        {
            new XmlNodeHandler(doc).ForFirstNodeFromPath(nodeName, action);
        }

        public static XmlNodeHandler GetHandlerForDocumentElement(this XmlDocument doc)
        {
            return new XmlNodeHandler(doc.DocumentElement);
        }

        public static XmlNodeHandler GetHandlerForDocumentElement(this XmlNode doc)
        {
            return (doc as XmlDocument).GetHandlerForDocumentElement();
        }

        public static string GetValueFromPath(this XmlNode baseNode, string path)
        {
            return baseNode.FindFirstNodeFromPath(path)?.InnerText;
        }

        public static T GetValueFromPath<T>(this XmlNode baseNode, string path)
        {
            return TypeHelper.Convert<T>(baseNode.FindFirstNodeFromPath(path)?.InnerText);
        }

        public static XmlNode FindFirstNodeFromPath(this XmlNode baseNode, string path)
        {
            string[] pathArray = path.Split('/');

            foreach (XmlNode item in baseNode.ChildNodes)
            {
                if (item.LocalName != pathArray[0])
                {
                    continue;
                }

                if (pathArray.Length == 1)
                {
                    return item;
                }

                var node = item.FindFirstNodeFromPath(path.Substring(pathArray[0].Length + 1));

                if (node != null)
                {
                    return node;
                }
            }

            return null;
        }

        public static IList<XmlNode> FindNodesFromPath(this XmlNode baseNode, string path)
        {
            List<XmlNode> nodes = new List<XmlNode>();
            string[] pathArray = path.Split('/');

            foreach (XmlNode item in baseNode.ChildNodes)
            {
                if (item.LocalName != pathArray[0])
                {
                    continue;
                }

                if (pathArray.Length == 1)
                {
                    nodes.Add(item);
                }
                else
                {
                    nodes.AddRange(item.FindNodesFromPath(path.Substring(pathArray[0].Length + 1)));
                }
            }

            return nodes;
        }

        public static XmlNode FindFirstNodeByAttribute(this XmlNode baseNode, string path, string attributeValue)
        {
            string[] pathArray = path.Split('/');

            foreach (XmlNode item in baseNode.ChildNodes)
            {
                if (item.LocalName != pathArray[0])
                {
                    continue;
                }

                if (pathArray.Length == 2)
                {
                    var attributeName = path.Substring(pathArray[0].Length + 1);

                    if (item.GetAttributeValue(attributeName) == attributeValue)
                    {
                        return item;
                    }
                }

                var node = item.FindFirstNodeByAttribute(path.Substring(pathArray[0].Length + 1), attributeValue);

                if (node != null)
                {
                    return node;
                }
            }

            return null;
        }

        public static string GetAttributeValue(this XmlNode baseNode, string nodeName, string attributeName)
        {
            foreach (XmlNode node in baseNode.ChildNodes)
            {
                string value = null;

                if (node.LocalName == nodeName)
                {
                    value = node.GetAttributeValue(attributeName);

                    if (value != null)
                    {
                        return value;
                    }
                }

                value = node.GetAttributeValue(nodeName, attributeName);

                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        public static string GetAttributeValue(this XmlNode baseNode, string attributeName)
        {
            foreach (XmlAttribute attr in baseNode.Attributes)
            {
                if (attr.LocalName == attributeName)
                {
                    return attr.InnerText ?? string.Empty;
                }
            }

            return null;
        }

        public static Stream ToStream(this XmlDocument doc)
        {
            MemoryStream xmlStream = new MemoryStream();
            doc.Save(xmlStream);
            return xmlStream;
        }

        public static IList<XmlNode> GetNodesByName(this XmlNode rootNode, string nodeName)
        {
            return rootNode.ChildNodes.OfType<XmlNode>().Where(c => c.LocalName == nodeName).ToList();
        }

        public static XmlNode GetNodeByName(this XmlNode rootNode, string nodeName)
        {
            for (int i = 0; i < rootNode.ChildNodes.Count; i++)
            {
                if (rootNode.ChildNodes[i].LocalName == nodeName)
                {
                    return rootNode.ChildNodes[i];
                }
            }

            return null;
        }

        public static string GetValueByNodeName(this XmlNode rootNode, string nodeName)
        {
            return rootNode.GetNodeByName(nodeName)?.InnerText;
        }

        public static XmlElement AddElementChildBefore(this XmlNode parentNode, XmlNode node, string nodeNde, string namespaceUri = null)
        {
            var element = namespaceUri == null ? parentNode.OwnerDocument.CreateElement(nodeNde) : parentNode.OwnerDocument.CreateElement(nodeNde, namespaceUri);
            parentNode.InsertBefore(element, node);
            return element;
        }

        public static XmlElement AddElementChild(this XmlNode parentNode, string nodeName, string namespaceUri = null)
        {
            var element = namespaceUri == null ? parentNode.OwnerDocument.CreateElement(nodeName) : parentNode.OwnerDocument.CreateElement(nodeName, namespaceUri);
            parentNode.AppendChild(element);
            return element;
        }

        public static void RemoveChilds(this XmlNode parentNode, string name)
        {
            foreach (var item in parentNode.ChildNodes.OfType<XmlNode>().Where(c => c.LocalName == name).ToList())
            {
                parentNode.RemoveChild(item);
            }
        }

        public static XmlElement AddTextNodeChild(this XmlNode parentNode, string nodeName, string text, string namespaceUri = null)
        {
            var element = parentNode.AddElementChild(nodeName, namespaceUri);
            var cdata = element.OwnerDocument.CreateTextNode(text);
            element.AppendChild(cdata);
            return element;
        }

        public static XmlElement AddCData(this XmlNode parentNode, string nodeName, string data, string namespaceUri = null)
        {
            var element = parentNode.AddElementChild(nodeName, namespaceUri);
            var cdata = element.OwnerDocument.CreateCDataSection(data);
            element.AppendChild(cdata);
            return element;
        }

        public static XmlDocument Sign(this XmlDocument xmlDocument, X509Certificate2 certificate, params string[] nodes)
        {
            var signer = new XmlDocumentSigner(certificate);

            foreach (var nodeName in nodes)
            {
                xmlDocument = signer.Sign(xmlDocument, nodeName);
            }

            return xmlDocument;
        }

        public static bool HasNode(this XmlNode node, string nodeName)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.LocalName == nodeName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
