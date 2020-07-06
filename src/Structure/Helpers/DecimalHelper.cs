using System.Globalization;

namespace Structure.Helpers
{
    public static class DecimalHelper
    {
        public static string ToString(decimal value, string decimalSeparator)
        {
            var nfi = new NumberFormatInfo { NumberDecimalSeparator = decimalSeparator };
            return value.ToString(nfi);
        }
    }
}
