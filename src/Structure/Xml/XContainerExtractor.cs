using System;
using System.Xml.Linq;

namespace Structure.Xml
{
    public class XContainerExtractor
    {
        private XContainer container;
        private XNamespace nameSpace;

        public XContainerExtractor(XContainer container, string nameSpace)
        {
            this.container = container;
            this.nameSpace = nameSpace;
        }
        
        public XContainerExtractor(XContainer container, XNamespace nameSpace)
        {
            this.container = container;
            this.nameSpace = nameSpace;
        }

        public XContainerExtractor(XContainer container)
        {
            this.container = container;
        }

        public void ForElements(string elementName, Action<XContainerExtractor> action)
        {
            var elements = nameSpace != null ? container.Elements(nameSpace + elementName) : container.Elements(elementName);

            foreach (var item in elements)
            {
                action(new XContainerExtractor(item, nameSpace));
            }
        }

        public void ForDescendants(string elementName, Action<XContainerExtractor> action)
        {
            var elements = nameSpace != null ? container.Descendants(nameSpace + elementName) : container.Descendants(elementName);

            foreach (var item in elements)
            {
                action(new XContainerExtractor(item, nameSpace));
            }
        }

        public string GetChildValue(string elementChildName)
        {
            return (container as XElement).GetChildValue(nameSpace + elementChildName);
        }

        public string GetAttributeValue(string attributeName)
        {
            return (container as XElement).GetAttributeValue(attributeName);
        }

        public T GetChildValue<T>(string elementChildName)
        {
            return (container as XElement).GetChildValue<T>(nameSpace + elementChildName);
        }

        public DateTime GetChildDateValue(string elementChildName)
        {
            return (container as XElement).GetChildDateValue(nameSpace + elementChildName);
        }

        public decimal GetChildDecimalValue(string elementChildName)
        {
            return (container as XElement).GetChildDecimalValue(nameSpace + elementChildName);
        }

        public T GetChildEnumValue<T>(string elementChildName)
            where T : struct
        {
            return (container as XElement).GetChildEnumValue<T>(nameSpace + elementChildName);
        }
    }
}
