using Structure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Structure.Extensions
{
    public static class StringExtensions
    {
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static bool In(this string str, params string[] valores)
        {
            foreach (var item in valores)
            {
                if (str == item)
                    return true;
            }

            return false;
        }

        public static bool NotIn(this string str, params string[] valores)
        {
            return !valores.Any(c => c == str);
        }

        public static string RemoveEmptySpaces(this string str)
        {
            return str.Replace(" ", string.Empty);
        }

        public static int ToIntDef(this string str, int def = 0)
        {
            if (!Int32.TryParse(str, out int number))
                return def;

            return number;
        }

        public static string ReplaceBeginning(this string str, int qtdCaracteres, string textoSubstituir)
        {
            return Regex.Replace(str, @"^[\w]{" + qtdCaracteres + "}", textoSubstituir);
        }

        public static string ReplaceAt(this string str, int index, string replace)
        {
            return ReplaceAt(str, index, replace.Length, replace);
        }

        public static string ReplaceAt(this string str, int index, int length, string replace)
        {
            return str.Remove(index, Math.Min(length, str.Length - index))
                .Insert(index, replace);
        }

        public static string CompleteWithLeadingZeros(this string texto, int qtdZeros, bool somenteValorInteiro = true)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return texto;
            }

            if (somenteValorInteiro && texto.Any(c => !Char.IsDigit(c)))
            {
                return texto;
            }

            return texto.PadLeft(qtdZeros, '0');
        }

        public static string InsertEndIfNotEqual(this string str, string strToInsert)
        {
            if (str.Length < strToInsert.Length)
            {
                return str;
            }

            var index = str.Length - strToInsert.Length;

            if (str.Substring(index, strToInsert.Length) != strToInsert)
            {
                str += strToInsert;
            }

            return str;
        }

        public static string InsertEndIfNotNullOrEmpty(this string str, string strInsert)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str += strInsert;
            }

            return str;
        }

        public static string InsertBeginIfNotEqual(this string str, string strInsert)
        {
            if (str.IndexOf(strInsert) != 0)
            {
                str = strInsert + str;
            }

            return str;
        }

        public static string Remove(this string str, string strToRemove)
        {
            return str.Replace(strToRemove, string.Empty);
        }

        public static string RemoveEnd(this string str, string strToRemove)
        {
            if (str.EndsWith(strToRemove))
            {
                str = str.Remove(str.LastIndexOf(strToRemove));
            }

            return str;
        }

        public static string RemoveEnd(this string str, int count)
        {
            return str.Remove(str.Length - count);
        }

        public static string RemoveBeginning(this string str, string strToRemove, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            if (str.StartsWith(strToRemove, stringComparison))
            {
                str = str.Remove(str.IndexOf(strToRemove, stringComparison), strToRemove.Length);
            }

            return str;
        }

        public static bool StartsWithIgnoreCaseAndDiacritics(this string a, string b)
        {
            return a.RemoveDiacritics().StartsWithIgnoreCase(b.RemoveDiacritics());
        }

        public static bool StartsWithIgnoreCase(this string a, string b)
        {
            return a.StartsWith(b, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool ContainsIgnoreCase(this string a, string b)
        {
            return a.ToLower().Contains(b.ToLower());
        }

        public static bool ContainsIgnoreCaseAndDiacritics(this string a, string b)
        {
            return a.ToLower().RemoveDiacritics().Contains(b.ToLower().RemoveDiacritics());
        }

        public static string RemoveDiacritics(this string a)
        {
            var enconding = Encoding.GetEncoding("iso-8859-8");
            return enconding.GetString(Encoding.Convert(Encoding.UTF8, enconding, Encoding.UTF8.GetBytes(a)));
        }

        public static string ConcatStringIf(this string str, Func<bool> predicate, string strToConcat)
        {
            return ConcatStringIf(str, predicate(), strToConcat);
        }

        public static string ConcatString(this string str, string strConcat)
        {
            return str + strConcat;
        }

        public static string ConcatStringIf(this string str, bool condition, string strToConcat)
        {
            if (condition)
            {
                str += strToConcat;
            }

            return str;
        }

        public static string ConcatStringIfOwnerNotNullOrEmpty(this string str, string strToConcat)
        {
            if (!StringHelper.NullOrEmpty(str))
            {
                str += strToConcat;
            }

            return str;
        }

        public static string ConcatStringIfNotNullOrEmpty(this string str, string strToConcat)
        {
            if (!StringHelper.NullOrEmpty(strToConcat))
            {
                str += strToConcat;
            }

            return str;
        }

        public static string[] Split(this string str, string separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return str.Split(new string[] { separator }, options);
        }

        public static List<string> ListSplit(this string str, string separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return str.Split(new string[] { separator }, options).ToList();
        }

        public static string[] SplitPos(this string str, string find)
        {
            int pi = str.IndexOf(find);
            IList<string> list = new List<string>();

            while (pi >= 0)
            {
                int pf = str.IndexOf(find, pi + 1);

                if (pf <= 0)
                {
                    pf = str.Length;
                }

                list.Add(str.Substring(pi, pf - pi));
            }

            return list.ToArray();
        }

        public static string RemoveFromEnd(this string str, params string[] suffixes)
        {
            return str.RemoveFromEnd(StringComparison.CurrentCulture, suffixes);
        }

        public static string RemoveFromEndIgnoreCase(this string str, params string[] suffixes)
        {
            return str.RemoveFromEnd(StringComparison.CurrentCultureIgnoreCase, suffixes);
        }

        public static string RemoveFromEnd(this string str, StringComparison stringComparison, params string[] suffixes)
        {
            for (int i = 0; i < suffixes.Length; i++)
            {
                if (str.EndsWith(suffixes[i], stringComparison))
                {
                    return str.Substring(0, str.Length - suffixes[i].Length);
                }
            }

            return str;
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static string CoalesceNullOrWhiteSpace(this string str, string anotherStr)
        {
            return str.IsNullOrWhiteSpace() ? anotherStr : str;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string ToKebabCase(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }

            var sb = new StringBuilder();

            foreach (var ch in str.ToCharArray())
            {
                if (char.IsUpper(ch))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("-");
                    }

                    sb.Append(char.ToLower(ch));
                }
                else
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString();
        }
    }
}
