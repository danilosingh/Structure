using Structure.Helpers;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Structure.Xml
{
    public static class XDocumentExtensions
    {
        public static T GetValue<T>(this XElement element)
        {
            return TypeHelper.Convert<T>(element.Value);
        }

        public static string GetValue(this XElement element)
        {
            return element.Value;
        }

        public static decimal GetDecimalValue(this XElement element)
        {
            return Convert.ToDecimal(element.Value);
        }

        public static decimal GetChildDecimalValue(this XElement element, string childElementName)
        {
            var elementFound = element.Elements(childElementName).FirstOrDefault();
            return elementFound != null ? elementFound.GetDecimalValue() : default(decimal);
        }

        public static string GetChildValue(this XElement element, string childElementName)
        {
            var elementFound = element.Elements(childElementName).FirstOrDefault();
            return elementFound != null ? elementFound.Value : default(string);
        }

        public static string GetChildValue(this XElement element, XName childElementXName)
        {
            var elementFound = element.Descendants(childElementXName).FirstOrDefault();
            return elementFound != null ? elementFound.Value : default(string);
        }

        public static T GetChildValue<T>(this XElement element, XName childElementXName)
        {
            var elementFound = element.Descendants(childElementXName).FirstOrDefault();
            return elementFound != null ?  TypeHelper.Convert<T>(elementFound.Value) : default(T);
        }

        public static string GetAttributeValue(this XElement element, string attributeName)
        {
            var att = element.Attributes(attributeName).FirstOrDefault();
            return att != null ? att.Value : null;
        }

        public static DateTime GetChildDateValue(this XElement element, XName childElementXName)
        {
            var elementFound = element.Descendants(childElementXName).FirstOrDefault();
            return elementFound != null ? DateTime.Parse(elementFound.Value) : default(DateTime);
        }

        public static decimal GetChildDecimalValue(this XElement element, XName childElementXName)
        {
            var elementFound = element.Descendants(childElementXName).FirstOrDefault();
            return elementFound != null ? decimal.Parse(elementFound.Value) : default(decimal);
        }

        public static T GetChildEnumValue<T>(this XElement element, XName childElementXName)
        {
            var elementFound = element.Descendants(childElementXName).FirstOrDefault();
            return elementFound != null ? (T)Enum.Parse(typeof(T), elementFound.Value) : default(T);
        }
    }
}
