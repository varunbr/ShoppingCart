using System.Collections.Generic;

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
                for (var j = 1; j <= limit && i + j < words.Length; j++)
                {
                    subWords.Add(string.Join(separator, words, i, j));
                }
            }
            return subWords.ToArray();
        }
    }
}
