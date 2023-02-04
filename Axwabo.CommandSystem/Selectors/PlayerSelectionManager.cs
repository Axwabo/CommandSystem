using System;
using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Exceptions;
using PlayerRoles.Spectating;
using PluginAPI.Core;
using RemoteAdmin;

namespace Axwabo.CommandSystem.Selectors;

public static class PlayerSelectionManager {

    public static CommandSender CurrentSender;

    private static readonly char[] StackSeparators = {':', '>', '.', '_', '-', ' '};
    private const string StackPrefix = "@stack";
    private const string SpectatedPrefix = "@spectated";
    private const string SpectatedPrefixShort = "@spec";

    public const string CommandExecutionFailedError = "Command execution failed! Error: ";
    public const string FailedToParsePlayerList = "Failed to parse player list: ";

    public static string EnsureNotEmpty(this string s, string message = null) => string.IsNullOrEmpty(s) ? throw new PlayerListProcessorException(message) : s;

    public static List<ReferenceHub> AllPlayers => ReferenceHub.AllHubs.Where(h => !h.isLocalPlayer).ToList();

    public static bool TryProcessPlayersCustom(ArraySegment<string> arguments, int startIndex, bool keepEmptyEntries, out List<ReferenceHub> targets, out string[] newArgs) {
        arguments = arguments.Segment(startIndex);
        if (arguments.Count == 0) {
            targets = HubCollection.Empty;
            newArgs = Array.Empty<string>();
            return false;
        }

        var formatted = string.Join(" ", arguments);
        try {
            if (TryDeterminePrefix(formatted, keepEmptyEntries, out targets, out newArgs))
                return true;
            if (arguments.At(0) != "*")
                return formatted.StartsWith("@")
                    ? AtSelectorProcessor.ProcessString(formatted.Substring(1), keepEmptyEntries, out targets, out newArgs)
                    : TryFindPlayerByName(arguments, out targets, out newArgs);
            targets = AllPlayers;
            newArgs = arguments.Segment(1).ToArray();
            return true;
        } catch (Exception e) {
            Log.Error($"Failed to parse player list! Content:\n{formatted}\nError:\n{Misc.RemoveStacktraceZeroes(e.ToString())}");
            if (e is PlayerListProcessorException)
                throw;
            throw new PlayerListProcessorException($"{e.GetType().FullName}: {e.Message}", e);
        }
    }

    private static bool TryFindPlayerByName(ArraySegment<string> arguments, out List<ReferenceHub> targets, out string[] args) {
        if (arguments.At(0).All(IsIdOrDot)) {
            targets = HubCollection.Empty;
            args = arguments.ToArray();
            return false;
        }

        var name = arguments.At(0);
        var player = AllPlayers.FirstOrDefault(p => p.nicknameSync.MyNick.ContainsIgnoreCase(name));
        if (player == null) {
            targets = HubCollection.Empty;
            args = arguments.ToArray();
            return false;
        }

        targets = new HubCollection(player);
        args = arguments.Segment(1).ToArray();
        return true;
    }

    private static bool IsIdOrDot(char character) => character == '.' || char.IsDigit(character) || char.IsWhiteSpace(character);

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
            newArgs = Split(formatted, keepEmptyEntries, true);
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

    public static string[] Split(string s, bool keepEmptyEntries, bool preTrimStart = false) {
        if (preTrimStart && s.Length > 0 && char.IsWhiteSpace(s[0]))
            s = s.Substring(1);
        return s.Split(new[] {' '}, keepEmptyEntries ? StringSplitOptions.None : StringSplitOptions.RemoveEmptyEntries);
    }

}
