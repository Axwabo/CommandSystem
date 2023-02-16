using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Structs;
using Utils;
#if !EXILED
using System.Linq;
#else
using Exiled.API.Features;
#endif

namespace Axwabo.CommandSystem;

public static class Extensions {

    public static string CombineNicknames(this IEnumerable<ReferenceHub> hubs, string separator = ", ")
        => string.Join(separator, hubs.Select(p => p.nicknameSync.MyNick));

    public static string CombineNicknames(this IEnumerable<CommandResultOnTarget> hubs, string separator = ", ")
        => string.Join(separator, hubs.Select(p => p.Target.nicknameSync.MyNick));

    public static string PluralizeWithCount(this string phrase, int count) => $"{count} {phrase.Pluralize(count)}";

    public static string Pluralize(this string phrase, int count) => count == 1 ? phrase : phrase + "s";

    public static string Substring(this string s, int start, params char[] separators) {
        var index = s.IndexOfAny(separators, start);
        return index == -1 ? s.Substring(start) : s.Substring(start, index - start);
    }

    public static string Substring(this string s, params char[] separators) => s.Substring(0, separators);

    public static bool ContainsIgnoreCase(this string s, string value) => s.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;

    public static bool TryParseIgnoreCase<T>(string value, out T result) where T : struct => Enum.TryParse(value.Trim(), true, out result);

    public static bool TryParseInt(string value, out int result) => int.TryParse(value.Trim(), out result);

    public static bool TryParseFloat(string value, out float result) => float.TryParse(value.Trim(), out result);

    public static List<ReferenceHub> GetTargets(this ArraySegment<string> arguments, out string[] newArgs, int startIndex = 0)
        => RAUtils.ProcessPlayerIdOrNamesList(arguments, startIndex, out newArgs);

    public static void SetFieldIfNotNull<T>(this T value, ref T field) where T : class {
        if (value != null)
            field = value;
    }

    public static void SafeCastAndSetIfNotNull<T>(this object value, ref T field) where T : class {
        if (field != null && value is T t)
            field = t;
    }

    public static bool AddIfNotNull<T>(this List<T> list, T value) where T : class {
        if (value == null)
            return false;
        list.Add(value);
        return true;
    }

}
