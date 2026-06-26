using System;
using System.Linq;

namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            string lower = input.ToLower();
            string cleaned = new string(lower.Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c)).ToArray());
            string reversed = new string(cleaned.Reverse().ToArray());
            return cleaned == reversed;
        }
    }
}