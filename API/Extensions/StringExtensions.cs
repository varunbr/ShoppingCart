using API.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Extensions
{
    public static class StringExtensions
    {
        public static string[] SubWords(this string input, string separator = " ", int limit = 3)
        {
            var words = input.Split(separator);
            var subWords = new List<string>();
            for (var i = 0; i < words.Length; i++)
            {
                for (var j = 1; j <= limit && i + j <= words.Length; j++)
                {
                    subWords.Add(string.Join(separator, words, i, j));
                }
            }
            return subWords.ToArray();
        }

        public static void GetRange(this string input, out int? from, out int? to)
        {
            from = null;
            to = null;

            try
            {
                var values = input.Split('-');

                if (values.Length != 2) return;

                from = string.IsNullOrWhiteSpace(values[0]) ? null : int.Parse(values[0]);
                to = string.IsNullOrWhiteSpace(values[1]) ? null : int.Parse(values[1]);
            }
            catch
            {
                // ignored
            }
        }

        public static void GetRange(this string input, out string from, out string to)
        {
            from = null;
            to = null;

            try
            {
                var values = input.Split('-');

                if (values.Length != 2) return;

                from = string.IsNullOrWhiteSpace(values[0]) ? null : values[0];
                to = string.IsNullOrWhiteSpace(values[1]) ? null : values[1];
            }
            catch
            {
                // ignored
            }
        }

        public static bool IsValidIntegerRange(this string input)
        {
            try
            {
                var values = input.Split('-');

                if (values.Length != 2) return false;

                if (!string.IsNullOrWhiteSpace(values[0]) && !int.TryParse(values[0], out _))
                    return false;
                if (!string.IsNullOrWhiteSpace(values[1]) && !int.TryParse(values[1], out _))
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ToStringError(this IEnumerable<IdentityError> errors)
        {
            return string.Join("\n", errors.Select(e => e.Description));
        }

        public static bool TryParseLocation(this string value, out string name, out string type)
        {
            name = type = null;
            try
            {
                if (string.IsNullOrEmpty(value)) return false;

                var values = value.Split('-');

                if (values.Length != 2 || !Enum.GetNames<LocationType>().Contains(values[1])) return false;

                name = values[0];
                type = values[1];
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
