using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Structure.Helpers
{
    public static class StringHelper
    {
        public static string EncodedString(Encoding encoding, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return encoding.GetString(encoding.GetBytes(str));
        }

        public static bool NullOrEmpty(string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }

        public static bool Smaller(string str, int length)
        {
            return NullOrEmpty(str) || str.Length < length;
        }

        public static string OnlyNumbers(string str)
        {
            return str == null ? null : string.Join(string.Empty, str.ToCharArray().Where(Char.IsDigit));
        }

        public static string EncondeUtf8(string str)
        {
            return EncodedString(Encoding.UTF8, str);
        }

        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public static bool Equals(string str1, string str2)
        {
            return str1 == str2;
        }

        public static string ToUpper(string str)
        {
            return !string.IsNullOrEmpty(str) ? str.ToUpper() : str;
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string GetInitials(string name, int maxLetters = 2, params string[] ignoredWords)
        {
            if (maxLetters <= 0)
            {
                throw new ArgumentException("Invalid maxLetters");
            }

            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            var items = name.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                .Where(c => !ignoredWords.Contains(c, StringComparer.OrdinalIgnoreCase))
                .ToArray();

            name = null;

            for (int i = 0; i < maxLetters && i < items.Length; i++)
            {
                var itemIndex = i == maxLetters - 1 ? items.Length - 1 : i;

                name += items[itemIndex].FirstOrDefault().ToString().ToUpper();
            }

            return name;
        }

        public static string Join(string separator, params object[] values)
        {
            return values != null ? string.Join(separator, values) : null;
        }

        public static bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

        public static string Repeat(string str, int count)
        {
            return string.Join(string.Empty, Enumerable.Repeat(str, count));
        }

        public static string LineBreak(int count)
        {
            return Repeat(Environment.NewLine, count);

        }
    }
}
