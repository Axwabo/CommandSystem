using System;

namespace Axwabo.CommandSystem;

public struct ValueRange<T> where T : IComparable {

    private const string Separator = "..";

    public static bool TryParse(string value, TryParse<T> valueParser, out ValueRange<T> range) {
        if (value.Contains(Separator))
            return TryParseWithRange(value, valueParser, out range);
        if (!valueParser(value, out var parsed)) {
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

    public static ValueRange<T> Parse(string value, TryParse<T> valueParser)
        => TryParse(value, valueParser, out var range)
            ? range
            : throw new FormatException($"Invalid range: {value}");

    private static bool TryParseWithRange(string value, TryParse<T> valueParser, out ValueRange<T> range) {
        T start = default;
        T end = default;
        var startSet = false;
        var endSet = false;
        if (value.StartsWith(Separator))
            endSet = valueParser(value.Substring(2), out end);
        else if (value.EndsWith(Separator))
            startSet = valueParser(value.Substring(0, value.Length - 2), out start);
        else {
            var splitIndex = value.IndexOf(Separator, StringComparison.Ordinal);
            startSet = valueParser(value.Substring(0, splitIndex), out start);
            endSet = valueParser(value.Substring(splitIndex + 2), out end);
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

    public T Start;

    public T End;

    public bool StartSpecified;

    public bool EndSpecified;

    public bool IsWithinRange(T item) => (!StartSpecified || item.CompareTo(Start) >= 0) && (!EndSpecified || item.CompareTo(End) <= 0);

}
