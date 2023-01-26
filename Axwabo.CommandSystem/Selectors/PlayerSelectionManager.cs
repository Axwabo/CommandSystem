using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Axwabo.CommandSystem.Selectors;

public static class PlayerSelectionManager {

    public static CommandSender CurrentSender;
    public const string CommandExecutionFailedError = "Command execution failed! Error: ";

    private static readonly char[] StackSeparators = {':', '>', '.', ' '};
    private const string StackPrefix = "@stack";

    public static bool TryProcessPlayersCustom(ArraySegment<string> arguments, int startIndex, bool keepEmptyEntries, out List<ReferenceHub> targets, out string[] newArgs) {
        var formatted = RAUtils.FormatArguments(arguments, startIndex);
        arguments = arguments.Segment(startIndex);
        if (formatted.StartsWith(StackPrefix, StringComparison.OrdinalIgnoreCase))
            return ProcessStack(formatted.Substring(StackPrefix.Length).TrimStart(), keepEmptyEntries, out targets, out newArgs);

        targets = null;
        newArgs = null;
        return false;
    }

    private static bool ProcessStack(string formatted, bool keepEmptyEntries, out List<ReferenceHub> targets, out string[] newArgs) {
        if (CurrentSender == null) {
            targets = null;
            newArgs = null;
            return false;
        }

        var stack = PlayerSelectionStack.Get(CurrentSender);
        if (formatted.Length < 1) {
            targets = stack.IsEmpty ? HubCollection.Empty : stack.Pop();
            newArgs = Array.Empty<string>();
            return true;
        }

        var sub = Split(formatted, keepEmptyEntries);
        if (formatted.IndexOfAny(StackSeparators) != 0) {
            targets = stack.IsEmpty ? HubCollection.Empty : stack.Pop();
            newArgs = sub.ToArray();
            return true;
        }

        var options = sub[0].TrimStart(StackSeparators);
        targets = !string.IsNullOrWhiteSpace(options)
            ? GetStackTargets(stack, options)
            : stack.IsEmpty
                ? HubCollection.Empty
                : stack.Pop();
        newArgs = sub.Segment(1).ToArray();
        return true;
    }

    private static List<ReferenceHub> GetStackTargets(PlayerSelectionStack stack, string options) {
        if (stack.IsEmpty)
            return HubCollection.Empty;
        return options.ToLower() switch {
            "first" or "f" => stack.Pop(),
            "last" or "l" => stack.PopAt(stack.LastIndex),
            "all" or "a" or "*" => stack.PopAll(),
            _ => ParseSpecialStackOptions(stack, options.ToLowerInvariant())
        };
    }

    private static List<ReferenceHub> ParseSpecialStackOptions(PlayerSelectionStack stack, string options)
        => !int.TryParse(options, out var index) || index < 0 || index >= stack.Count
            ? HubCollection.Empty
            : stack.PopAt(index);

    private static string[] Split(string s, bool keepEmptyEntries) => s.Split(new[] {' '}, keepEmptyEntries ? StringSplitOptions.None : StringSplitOptions.RemoveEmptyEntries);

}
