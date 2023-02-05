using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Axwabo.CommandSystem;

public static class Extensions {

    public static string CombineNicknames(this IEnumerable<ReferenceHub> hubs, string separator = ", ")
        => string.Join(separator, hubs.Select(p => p.nicknameSync.MyNick));

    public static string Pluralize(this string word, int count) => $"{count} {(count == 1 ? word : word + "s")}";

    public static string Substring(this string s, int start, params char[] separators) {
        var index = s.IndexOfAny(separators, start);
        return index == -1 ? s.Substring(start) : s.Substring(start, index - start);
    }

    public static string Substring(this string s, params char[] separators) => s.Substring(0, separators);

    public static bool ContainsIgnoreCase(this string s, string value) => s.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;

    public static bool TryParseIgnoreCase<T>(string value, out T result) where T : struct => Enum.TryParse(value, true, out result);

    public static List<ReferenceHub> GetTargets(this ArraySegment<string> arguments, out string[] newArgs, int startIndex = 0)
        => RAUtils.ProcessPlayerIdOrNamesList(arguments, startIndex, out newArgs);

    public static void SetFieldIfNotNull<T>(this T value, ref T field) where T : class {
        if (value != null)
            field = value;
    }

}
