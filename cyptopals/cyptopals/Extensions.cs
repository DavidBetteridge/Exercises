using System.Collections.Generic;

namespace cryptopals
{
    public static class Extensions
    {
        internal static string ConcatStrings(this IEnumerable<string> strings) => string.Join("", strings);
        internal static string ConcatStrings(this IEnumerable<char> strings) => string.Join("", strings);
    }
}
