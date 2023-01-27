using System;
using System.Collections.Generic;
using System.Linq;
using PlayerRoles.Spectating;
using RemoteAdmin;
using Utils;

namespace Axwabo.CommandSystem.Selectors;

public static class PlayerSelectionManager {

    public static CommandSender CurrentSender;
    public const string CommandExecutionFailedError = "Command execution failed! Error: ";

    private static readonly char[] StackSeparators = {':', '>', '.', '_', '-', ' '};
    private const string StackPrefix = "@stack";
    private const string SpectatedPrefix = "@spectated";
    private const string SpectatedPrefixShort = "@spec";

    public static List<ReferenceHub> AllPlayers => ReferenceHub.AllHubs.Where(h => !h.isLocalPlayer).ToList();

    public static bool TryProcessPlayersCustom(ArraySegment<string> arguments, int startIndex, bool keepEmptyEntries, out List<ReferenceHub> targets, out string[] newArgs) {
        var formatted = RAUtils.FormatArguments(arguments, startIndex);
        arguments = arguments.Segment(startIndex);
        if (TryDeterminePrefix(formatted, keepEmptyEntries, out targets, out newArgs))
            return true;
        if (arguments.Count > 0 && arguments.At(0) == "*") {
            targets = AllPlayers;
            newArgs = arguments.Segment(1).ToArray();
            return true;
        }

        if (formatted.StartsWith("@"))
            return CharSelectorProcessor.ProcessString(formatted, keepEmptyEntries, out targets, out newArgs);
        targets = HubCollection.Empty;
        newArgs = arguments.ToArray();
        return false;
    }

    private static bool TryDeterminePrefix(string formatted, bool keepEmptyEntries, out List<ReferenceHub> targets, out string[] newArgs) {
        if (formatted.StartsWith(StackPrefix, StringComparison.OrdinalIgnoreCase)) {
            ProcessStack(formatted.Substring(StackPrefix.Length).TrimStart(), keepEmptyEntries, out targets, out newArgs);
            return true;
        }

        if (formatted.StartsWith(SpectatedPrefix, StringComparison.OrdinalIgnoreCase)) {
            GetSpectated(formatted.Substring(SpectatedPrefix.Length).TrimStart(), keepEmptyEntries, out targets, out newArgs);
            return true;
        }

        if (formatted.StartsWith(SpectatedPrefixShort, StringComparison.OrdinalIgnoreCase)) {
            GetSpectated(formatted.Substring(SpectatedPrefixShort.Length).TrimStart(), keepEmptyEntries, out targets, out newArgs);
            return true;
        }

        targets = null;
        newArgs = null;
        return false;
    }

    private static void GetSpectated(string formatted, bool keepEmptyEntries, out List<ReferenceHub> targets, out string[] newArgs) {
        if (CurrentSender is not PlayerCommandSender {ReferenceHub: {roleManager.CurrentRole: SpectatorRole} hub}) {
            targets = HubCollection.Empty;
            newArgs = Split(formatted, keepEmptyEntries);
            return;
        }

        foreach (var target in AllPlayers) {
            if (!target.IsSpectatedBy(hub))
                continue;
            targets = new HubCollection(target);
            newArgs = Split(formatted, keepEmptyEntries);
            return;
        }

        targets = HubCollection.Empty;
        newArgs = Split(formatted, keepEmptyEntries);
    }

    private static void ProcessStack(string formatted, bool keepEmptyEntries, out List<ReferenceHub> targets, out string[] newArgs) {
        if (CurrentSender == null) {
            targets = HubCollection.Empty;
            newArgs = Split(formatted, keepEmptyEntries);
            return;
        }

        var stack = PlayerSelectionStack.Get(CurrentSender);
        if (formatted.Length < 1) {
            targets = stack.IsEmpty ? HubCollection.Empty : stack.Pop();
            newArgs = Array.Empty<string>();
            return;
        }

        var sub = Split(formatted, keepEmptyEntries);
        if (formatted.IndexOfAny(StackSeparators) != 0) {
            targets = stack.IsEmpty ? HubCollection.Empty : stack.Pop();
            newArgs = sub.ToArray();
            return;
        }

        var options = sub[0].TrimStart(StackSeparators);
        targets = !string.IsNullOrWhiteSpace(options)
            ? GetStackTargets(stack, options)
            : stack.IsEmpty
                ? HubCollection.Empty
                : stack.Pop();
        newArgs = sub.Segment(1).ToArray();
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
