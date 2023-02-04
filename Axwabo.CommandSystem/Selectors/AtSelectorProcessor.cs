using System;
using System.Collections.Generic;
using System.Text;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Selectors.Filtering;
using PlayerRoles;
using PluginAPI.Core;
using RemoteAdmin;
using UnityEngine;

namespace Axwabo.CommandSystem.Selectors;

public static class AtSelectorProcessor {

    private static readonly char[] ValidChars = {'a', 'r', 's', 'o', 'A', 'R', 'S', 'O'};

    public static bool ProcessString(string formatted, bool keepEmptyEntries, out List<ReferenceHub> targets, out string[] newArgs) {
        char selectorChar;
        if (formatted.Length < 1 || !IsValidChar(selectorChar = formatted[0])) {
            targets = null;
            newArgs = null;
            return false;
        }

        if (formatted.Length < 3) {
            targets = ExecuteSelector(selectorChar, null, -1);
            newArgs = Array.Empty<string>();
            return true;
        }

        if (formatted[1] != '[') {
            targets = ExecuteSelector(selectorChar, null, -1);
            newArgs = PlayerSelectionManager.Split(formatted.Substring(1), keepEmptyEntries, true);
            return true;
        }

        var state = new ParserState {
            FullString = formatted,
            EndIndex = 3
        };

        for (var i = 2; i < formatted.Length; i++)
            if (ProcessChar(i, state))
                break;

        targets = ExecuteSelector(selectorChar, state.Filters, state.Limit);
        newArgs = state.EndIndex >= formatted.Length
            ? Array.Empty<string>()
            : PlayerSelectionManager.Split(formatted.Substring(state.EndIndex + 1), keepEmptyEntries, true);
        return true;
    }

    public static HubFilter GetFilter(string name, string value, ref int limit, bool inverted = false) {
        if (string.IsNullOrEmpty(name))
            return null;
        var alias = name.ToLower();
        var filter = alias switch {
            "limit" => ParseLimit(value, out limit),
            "role" or "class" or "r" or "c" => PresetHubFilters.Role(value),
            "playerid" or "pid" => PresetHubFilters.Id(value),
            "nickname" or "nick" or "name" => PresetHubFilters.Nickname(value),
            "alive" => PlayerRolesUtils.IsAlive,
            "remoteadmin" or "ra" => PresetHubFilters.RemoteAdmin,
            "onstack" or "stack" => PresetHubFilters.Stack,
            _ => CustomHubFilterRegistry.Get(alias) ?? throw new PlayerListProcessorException($"Unknown player filter: {name}")
        };
        return inverted ? filter.Invert() : filter;
    }

    private static HubFilter ParseLimit(string value, out int limit) {
        limit = value switch {
            "all" => Player.Count,
            "half" => Mathf.CeilToInt(Player.Count / 2f),
            "quarter" => Mathf.CeilToInt(Player.Count / 4f),
            _ => int.TryParse(value, out var x) ? x : ParseFractionLimit(value)
        };
        return null;
    }

    private static int ParseFractionLimit(string value) {
        if (!value.Contains("/"))
            return int.TryParse(value, out var limit) ? limit : -1;
        var split = value.Split('/');
        return split.Length < 2 || !int.TryParse(split[0], out var numerator) || !int.TryParse(split[1], out var denominator)
            ? -1
            : Mathf.CeilToInt(Player.Count * (numerator / (float) denominator));
    }

    public static List<ReferenceHub> ExecuteSelector(char selectorChar, List<HubFilter> filters, int limit) => GetDefaultTargets(
        GetAllFiltered(filters),
        selectorChar,
        limit
    );

    private static List<ReferenceHub> GetAllFiltered(List<HubFilter> filters) {
        var all = PlayerSelectionManager.AllPlayers;
        if (filters == null)
            return all;
        for (var i = 0; i < all.Count; i++) {
            if (MatchesAllFilters(all[i], filters))
                continue;
            all.RemoveAt(i);
            i--;
        }

        return all;
    }

    private static bool MatchesAllFilters(ReferenceHub hub, List<HubFilter> filters) {
        if (filters.Count == 0)
            return true;
        foreach (var f in filters)
            if (f != null && !f(hub))
                return false;
        return true;
    }

    private static List<ReferenceHub> GetDefaultTargets(List<ReferenceHub> candidates, char selector, int limit) => selector switch {
        'a' or 'A' => candidates,
        'r' or 'R' => GetRandom(candidates, limit),
        's' or 'S' => Self,
        'o' or 'O' => Others,
        _ => throw new ArgumentOutOfRangeException(nameof(selector), selector, null)
    };

    private static List<ReferenceHub> GetRandom(List<ReferenceHub> all, int limit) {
        if (all.Count == 0)
            return HubCollection.Empty;
        if (limit < 0)
            return new HubCollection(all.RandomItem());
        var list = new HubCollection(limit);
        for (var i = 0; i < limit; i++) {
            var count = all.Count;
            if (count == 0)
                break;
            var index = UnityEngine.Random.Range(0, count);
            list.Add(all[index]);
            all.RemoveAt(index);
        }

        return list;
    }

    private static List<ReferenceHub> Self => HubCollection.From(PlayerSelectionManager.CurrentSender);

    private static List<ReferenceHub> Others {
        get {
            var list = PlayerSelectionManager.AllPlayers;
            if (PlayerSelectionManager.CurrentSender is PlayerCommandSender {ReferenceHub: var hub})
                list.Remove(hub);
            return list;
        }
    }

    public static bool IsValidChar(char c) => ValidChars.Contains(c);

    private static bool ProcessChar(int index, ParserState state) {
        var escaped = state.EscapeNext;
        var c = state.FullString[index];
        if (escaped) {
            state.EscapeNext = false;
            state.Builder.Append(c);
            return false;
        }

        if (index == state.FullString.Length - 1)
            return CloseSelector(index, state);

        switch (c) {
            case ']':
                return CloseSelector(index, state);
            case '\\':
                state.EscapeNext = true;
                return false;
            case '=':
                state.SetValueFromBuilder().IsReadingValue = true;
                return false;
            case '!':
                if (state.Builder.Length == 0 || state.CharAt(index + 1) == '=')
                    state.Invert = true;
                else
                    state.Builder.Append(c);
                return false;
            case ',':
                state.SetValueFromBuilder().IsReadingValue = false;
                state.AddFilter();
                state.Invert = false;
                state.FilterName = state.FilterValue = "";
                return false;
            default:
                state.Builder.Append(c);
                return false;
        }
    }

    private static bool CloseSelector(int index, ParserState state) {
        state.SetValueFromBuilder().IsReadingValue = false;
        state.AddFilter();
        state.Invert = false;
        state.EndIndex = index;
        return true;
    }

    private sealed class ParserState {

        public int Limit = -1;

        public string FullString;

        public readonly StringBuilder Builder = new();

        public readonly List<HubFilter> Filters = new();

        public int EndIndex;

        public string FilterName;

        public string FilterValue;

        public bool Invert;

        public bool IsReadingValue;

        public bool EscapeNext;

        public ParserState SetValueFromBuilder() {
            if (IsReadingValue)
                FilterValue = Builder.ToString();
            else
                FilterName = Builder.ToString();
            Builder.Clear();
            return this;
        }

        public int CharAt(int index) => index >= FullString.Length ? -1 : FullString[index];

        public void AddFilter() => Filters.Add(GetFilter(FilterName?.Trim(), FilterValue, ref Limit, Invert));

    }

}
