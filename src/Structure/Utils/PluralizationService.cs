using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Structure.Utils
{
    public class PluralizationService : IPluralizationService
    {
        private static readonly string[] unpluralizables = new string[] { "auth", "equipment", "information", "rice", "money", "species", "series", "fish", "sheep", "deer" };

        private static readonly IDictionary<string, string> pluralizations = new Dictionary<string, string>
        {
            { "person", "people" },
            { "ox", "oxen" },
            { "child", "children" },
            { "foot", "feet" },
            { "tooth", "teeth" },
            { "goose", "geese" },
            { "(.*)fe?", "$1ves" },
            { "(.*)man$", "$1men" },
            { "(.+[aeiou]y)$", "$1s" },
            { "(.+[^aeiou])y$", "$1ies" },
            { "(.+z)$", "$1zes" },
            { "([m|l])ouse$", "$1ice" },
            { "(.+)(e|i)x$", "$1ices"},
            { "(octop|vir)us$", "$1i"},
            { "(.+[^aeiou])is$", "$1es" },
            { "(.+(s|x|sh|ch))$", "$1es"},
            { "(.+)", @"$1s" }
        };

        public string Pluralize(string word)
        {
            if (unpluralizables.Contains(word, StringComparer.InvariantCultureIgnoreCase))
                return word;

            var plural = "";

            foreach (var pluralization in pluralizations)
            {
                if (Regex.IsMatch(word, pluralization.Key))
                {
                    plural = Regex.Replace(word, pluralization.Key, pluralization.Value);
                    break;
                }
            }

            return plural;
        }
    }
}
