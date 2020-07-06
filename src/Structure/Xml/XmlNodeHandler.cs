using Structure.Helpers;
using System;
using System.Globalization;
using System.Xml;

namespace Structure.Xml
{
    public class XmlNodeHandler
    {
        private CultureInfo cultureInfo;

        public XmlNode Node { get; private set; }

        public XmlNodeHandler(XmlNode node, string decimalSeparator = ".")
        {
            Node = node;
            cultureInfo = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            cultureInfo.NumberFormat.NumberDecimalSeparator = decimalSeparator;
        }

        public bool ForNodes(string nodeName, Action<XmlNodeHandler> action)
        {
            bool found = false;

            foreach (XmlNode item in Node.ChildNodes)
            {
                if (item.Name == nodeName)
                {
                    action(new XmlNodeHandler(item));
                    found = true;
                }
            }

            return found;
        }

        public bool ForNodesFromPath(string path, Action<XmlNodeHandler> action)
        {
            bool found = false;

            foreach (var item in Node.GetHandlersFromPath(path))
            {
                action(item);
                found = true;
            }

            return found;
        }

        public void RemoveAll()
        {
            Node.RemoveAll();
        }

        public bool ForFirstNode(string nodeName, Action<XmlNodeHandler> action)
        {
            var extractor = GetHandler(nodeName);

            if (extractor != null)
            {
                action(extractor);
                return true;
            }

            return false;
        }

        public bool ForFirstNodeFromPath(string path, Action<XmlNodeHandler> action)
        {
            var extractor = Node.GetHandlerFromPath(path);

            if (extractor != null)
            {
                action(extractor);
                return true;
            }

            return false;
        }

        public XmlNodeHandler GetHandler(string nodeName)
        {
            return Node.GetHandler(nodeName);
        }

        public XmlNodeHandler GetHandlerFromPath(string nodePath)
        {
            return Node.GetHandlerFromPath(nodePath);
        }

        public XmlNode GetNodeFromPath(string nodePath)
        {
            return Node.FindFirstNodeFromPath(nodePath);
        }

        public XmlNode GetNode(string nodeName)
        {
            return Node.GetNodeByName(nodeName);
        }

        public string GetValue(string childNodeName)
        {
            for (int i = 0; i < Node.ChildNodes.Count; i++)
            {
                var item = Node.ChildNodes[i];

                if (item.LocalName == childNodeName)
                {
                    return item.InnerText;
                }
            }

            return default;
        }

        public string GetValueFromPath(XmlNode node, string childNodePath)
        {
            string[] pathArray = childNodePath.Split('/');

            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.LocalName != pathArray[0])
                {
                    continue;
                }

                if (pathArray.Length == 1)
                {
                    return item.InnerText;
                }

                return GetValueFromPath(item, childNodePath.Substring(pathArray[0].Length + 1));
            }

            return null;
        }

        public string GetValueFromPath(string nodePath)
        {
            return GetValueFromPath(Node, nodePath);
        }

        public T GetValueFromPath<T>(string nodePath)
        {
            return TypeHelper.Convert<T>(GetValueFromPath(Node, nodePath));
        }

        public decimal GetDecimalValue(string childNodeName)
        {            
            decimal.TryParse(GetValue(childNodeName), NumberStyles.Any, cultureInfo, out var result);
            return result;
        }

        public int GetIntValue(string childNodeName)
        {
            int.TryParse(GetValue(childNodeName), out int result);
            return result;
        }

        public long GetLongValue(string childNodeName)
        {
            return Convert.ToInt64(GetValue(childNodeName));
        }

        public DateTime GetDateValue(string childNodeName)
        {
            return Convert.ToDateTime(GetValue(childNodeName));
        }

        public DateTime GetUtcDateValue(string childNodeName)
        {
            return DateTimeOffset.Parse(GetValue(childNodeName)).DateTime;
        }

        public T GetEnumValue<T>(string childNodeName)
        {
            var strValue = GetValue(childNodeName);
            return strValue != null ? (T)Enum.Parse(typeof(T), strValue) : default(T);
        }

        public T GetValue<T>(string childNodeName)
        {
            return TypeHelper.Convert<T>(GetValue(childNodeName));
        }

        public string GetAttributeValue(string attributeName)
        {
            foreach (XmlAttribute item in Node.Attributes)
            {
                if (item.Name == attributeName)
                {
                    return item.Value;
                }
            }

            return null;
        }

        public void SetValue(string path, string value)
        {
            var node = Node.FindFirstNodeFromPath(path);

            if (node != null)
            {
                node.Value = value;
            }
        }

        public void SetValue(string path, string[] childNodes, string value)
        {
            var node = Node.FindFirstNodeFromPath(path);

            foreach (var nodeName in childNodes)
            {
                var childNome = node.FindFirstNodeFromPath(nodeName);

                if (childNome != null)
                {
                    childNome.Value = value;
                }
            }
        }

        public void SetValue(string[] childNodes, string value)
        {
            foreach (var nodeName in childNodes)
            {
                var childNome = Node.FindFirstNodeFromPath(nodeName);

                if (childNome != null)
                {
                    childNome.Value = value;
                }
            }
        }

        public void SetInnerText(string[] childNodes, object value)
        {
            foreach (var nodeName in childNodes)
            {
                SetInnerText(nodeName, value);
            }
        }

        public void SetInnerText(string path, object value)
        {
            var node = Node.FindFirstNodeFromPath(path);

            if (node != null)
            {
                node.InnerText = value?.ToString();
            }            
        }
        
        public void SetValue(string path, int value)
        {
            SetValue(path, Convert.ToString(value));
        }

        public void SetAttributeValue(string attributeName, string value)
        {
            foreach (XmlAttribute item in Node.Attributes)
            {
                if (item.Name == attributeName)
                {
                    item.Value = value;
                    return;
                }
            }
        }

        public XmlNodeHandler GetParentNodeHandler()
        {
            return new XmlNodeHandler(Node.ParentNode, cultureInfo.NumberFormat.NumberDecimalSeparator);
        }

        public void CreateNode(string nodeName, string value)
        {
            var newNode = Node.OwnerDocument.CreateElement(Node.Prefix, nodeName, Node.NamespaceURI);
            newNode.InnerText = value;
            Node.AppendChild(newNode);

        }

        public XmlDocument GetXmlDocument()
        {
            return Node as XmlDocument;
        }
    }
}
