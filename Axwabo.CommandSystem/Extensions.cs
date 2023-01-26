using System.Collections.Generic;
using System.Linq;

namespace Axwabo.CommandSystem;

public static class Extensions {

    public static string CombineNicknames(this IEnumerable<ReferenceHub> hubs, string separator = ", ") => string.Join(separator, hubs.Select(p => p.nicknameSync.MyNick));

    public static string Pluralize(this string word, int count) => $"{count} {(count == 1 ? word : word + "s")}";

    public static string Substring(this string s, int start, params char[] separators) {
        var index = s.IndexOfAny(separators, start);
        return index == -1 ? s.Substring(start) : s.Substring(start, index - start);
    }

    public static string Substring(this string s, params char[] separators) => s.Substring(0, separators);

}
