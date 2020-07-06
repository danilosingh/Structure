using System.Text.RegularExpressions;

namespace Structure.Helpers
{
    public static class XmlHelper
    {
        public static string SetTagValue(string xml, string tag, string value)
        {
            return Regex.Replace(xml, string.Format("<{0}>(.*)</{0}>", tag), string.Format("<{0}>{1}</{0}>", tag, value));
        }

        public static string GetTagValue(string xml, string tag)
        {
            Regex regex = new Regex(string.Format("<{0}>(.*)</{0}>", tag));
            var math = regex.Match(xml);
            return math.Success ? math.Groups[1].ToString() : null;
        }

        public static string GetAttributeValue(string xml, string attrName)
        {
            Regex regex = new Regex(string.Format(@"{0}=""(.[^""]+)", attrName));
            var math = regex.Match(xml);
            return math.Success ? math.Groups[1].ToString() : null;
        }

        public static string SetAttributeValue(string xml, string attrName, string attrValue)
        {
            return Regex.Replace(xml, string.Format(@"{0}=""(.[^""]+)""", attrName), string.Format("{0}=\"{1}\"", attrName, attrValue));
        }        
    }
}
