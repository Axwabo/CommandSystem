using System;

namespace Axwabo.CommandSystem.Structs;

public struct ValueRange<T> where T : IComparable {

    #region Parse

    private const string Separator = "..";

    public static bool TryParse(string value, TryParseDelegate<T> valueParser, out ValueRange<T> range)
        => TryParseInternal(value, valueParser, out range, false);

    private static bool TryParseInternal(string value, TryParseDelegate<T> valueParser, out ValueRange<T> range, bool throwOnInvalid) {
        if (value.Contains(Separator))
            return TryParseWithRange(value, valueParser, out range, throwOnInvalid);
        if (!valueParser(value, out var parsed)) {
            if (throwOnInvalid)
                throw new FormatException($"Invalid value for single value range (type {typeof(T).FullName}): {value}");
            range = default;
            return false;
        }

        range = new ValueRange<T> {
            StartSpecified = true,
            EndSpecified = true,
            Start = parsed,
            End = parsed
        };
        return true;
    }

    public static ValueRange<T> Parse(string value, TryParseDelegate<T> valueParser)
        => TryParseInternal(value, valueParser, out var range, true)
            ? range
            : throw new FormatException($"Invalid range: {value}");

    private static bool TryParseWithRange(string value, TryParseDelegate<T> valueParser, out ValueRange<T> range, bool throwOnInvalid) {
        T start = default;
        T end = default;
        var startSet = false;
        var endSet = false;
        if (value.StartsWith(Separator))
            endSet = ParseValue(value.Substring(2), nameof(end), valueParser, throwOnInvalid, out end);
        else if (value.EndsWith(Separator))
            startSet = ParseValue(value.Substring(0, value.Length - 2), nameof(start), valueParser, throwOnInvalid, out start);
        else {
            var splitIndex = value.IndexOf(Separator, StringComparison.Ordinal);
            startSet = ParseValue(value.Substring(0, splitIndex), nameof(start), valueParser, throwOnInvalid, out start);
            endSet = ParseValue(value.Substring(splitIndex + 2), nameof(end), valueParser, throwOnInvalid, out end);
        }

        if (!startSet && !endSet) {
            range = default;
            return false;
        }

        range = new ValueRange<T> {
            StartSpecified = startSet,
            EndSpecified = endSet,
            Start = start,
            End = end
        };
        return true;
    }

    private static bool ParseValue(string value, string variableName, TryParseDelegate<T> valueParser, bool throwOnInvalid, out T start) {
        var startSet = valueParser(value, out start);
        if (!startSet && throwOnInvalid)
            throw new FormatException($"Invalid value for {variableName} (type {typeof(T).FullName}): {value}");
        return startSet;
    }

    #endregion

    #region Members

    public T Start;

    public T End;

    public bool StartSpecified;

    public bool EndSpecified;

    public bool IsWithinRange(T item) => (!StartSpecified || item.CompareTo(Start) >= 0) && (!EndSpecified || item.CompareTo(End) <= 0);

    #endregion

}
