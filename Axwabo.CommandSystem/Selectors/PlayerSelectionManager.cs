using System;
using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Exceptions;
using PlayerRoles;
using PlayerRoles.Spectating;
using RemoteAdmin;

namespace Axwabo.CommandSystem.Selectors;

/// <summary>
/// Manages custom player selectors.
/// </summary>
public static class PlayerSelectionManager
{

    /// <summary>The current <see cref="CommandSender"/> executing a command.</summary>
    public static CommandSender CurrentSender;

    private static readonly char[] StackSeparators = {':', '>', '_', '-', ' '};
    private const string StackPrefix = "@stack";
    private const string SpectatedPrefix = "@spectated";
    private const string SpectatedPrefixShort = "@spec";

    /// <summary>The message displayed when a command throws an exception.</summary>
    public const string CommandExecutionFailedError = "Command execution failed! Error: ";

    /// <summary>The message displayed when the supplied player list cannot be parse.</summary>
    public const string FailedToParsePlayerList = "Failed to parse player list: ";

    /// <summary>
    /// Ensures that the given string is not empty.
    /// </summary>
    /// <param name="s">The string to check.</param>
    /// <param name="message">The message to create the exception with.</param>
    /// <returns>The string itself.</returns>
    /// <exception cref="PlayerListProcessorException">Thrown when the string is null or empty.</exception>
    public static string EnsureNotEmpty(this string s, string message = null)
        => !string.IsNullOrEmpty(s)
            ? s
            : message == null
                ? throw new PlayerListProcessorException()
                : throw new PlayerListProcessorException(message);

    private static bool NonHost(ReferenceHub h) => !h.isLocalPlayer || Plugin.Instance.Config.Debug;

    /// <summary>Gets all players.</summary>
    public static List<ReferenceHub> AllPlayers => ReferenceHub.AllHubs.Where(NonHost).ToList();

    /// <summary>Gets all alive players.</summary>
    public static List<ReferenceHub> NonSpectators => ReferenceHub.AllHubs.Where(h => !h.isLocalPlayer && h.IsAlive()).ToList();

    /// <summary>Gets the player count.</summary>
    public static int PlayerCount =>
#if EXILED
        Exiled.API.Features.Server.PlayerCount;
#else
        PluginAPI.Core.Player.Count;
#endif

    /// <summary>
    /// Attempts to process the given argument array as a player list with the custom selectors.
    /// </summary>
    /// <param name="arguments">The arguments to process.</param>
    /// <param name="startIndex">The index to start processing from.</param>
    /// <param name="keepEmptyEntries">Whether to keep empty entries in the argument array.</param>
    /// <param name="targets">The parsed list of targets.</param>
    /// <param name="newArgs">The new argument array.</param>
    /// <returns>Whether the custom processing was successful.</returns>
    /// <exception cref="PlayerListProcessorException">Re-thrown upon an exception.</exception>
    public static bool TryProcessPlayersCustom(ArraySegment<string> arguments, int startIndex, bool keepEmptyEntries, out List<ReferenceHub> targets, out string[] newArgs)
    {
        arguments = arguments.Segment(startIndex);
        if (arguments.Count == 0)
        {
            targets = HubCollection.Empty;
            newArgs = Array.Empty<string>();
            return false;
        }

        var formatted = arguments.Join();
        try
        {
            if (arguments.At(0) != "*")
                return formatted.StartsWith("@")
                    ? AtSelectorProcessor.ProcessString(formatted.Substring(1), keepEmptyEntries, out targets, out newArgs)
                    : TryFindPlayerByName(arguments, out targets, out newArgs);
            targets = AllPlayers;
            newArgs = arguments.Segment(1).ToArray();
            return true;
        }
        catch (Exception e)
        {
            Log.Error($"Failed to parse player list! Content:\n{formatted}\nError:\n{Misc.RemoveStacktraceZeroes(e.ToString())}");
            if (e is PlayerListProcessorException)
                throw;
            throw new PlayerListProcessorException(e.Message, e);
        }
    }

    /// <summary>
    /// Attempts to process the given string as a player list with the custom selectors.
    /// </summary>
    /// <param name="query">The string to process.</param>
    /// <param name="hubList">The list of targets to add to.</param>
    public static void ParseSingleQuery(string query, List<ReferenceHub> hubList)
    {
        if (string.IsNullOrEmpty(query))
            return;
        var trimmed = query.Trim();
        if (int.TryParse(trimmed, out var id) && ReferenceHub.TryGetHub(id, out var hub))
        {
            if (!hubList.Contains(hub))
                hubList.Add(hub);
            return;
        }

        if (!TryUseStandaloneProcessor(trimmed, false, out var targets, out _))
            return;
        foreach (var target in targets)
            if (!hubList.Contains(target))
                hubList.Add(target);
    }

    private static bool TryFindPlayerByName(ArraySegment<string> arguments, out List<ReferenceHub> targets, out string[] args)
    {
        if (arguments.At(0).All(IsIdOrDot))
        {
            targets = HubCollection.Empty;
            args = arguments.ToArray();
            return false;
        }

        var name = arguments.At(0);
        var player = AllPlayers.FirstOrDefault(p => p.nicknameSync.MyNick.ContainsIgnoreCase(name));
        if (player == null)
        {
            targets = HubCollection.Empty;
            args = arguments.ToArray();
            return false;
        }

        targets = new HubCollection(player);
        args = arguments.Segment(1).ToArray();
        return true;
    }

    private static bool IsIdOrDot(char character) => character == '.' || char.IsDigit(character) || char.IsWhiteSpace(character);

    /// <summary>
    /// Attempts to use a standalone player list processor.
    /// </summary>
    /// <param name="formatted">The string to process.</param>
    /// <param name="keepEmptyEntries">Whether to keep empty entries in the argument array.</param>
    /// <param name="targets">The parsed list of targets.</param>
    /// <param name="newArgs">The new argument array.</param>
    /// <returns>Whether the custom processing was successful.</returns>
    public static bool TryUseStandaloneProcessor(string formatted, bool keepEmptyEntries, out List<ReferenceHub> targets, out string[] newArgs)
    {
        if (formatted.StartsWith(StackPrefix, StringComparison.OrdinalIgnoreCase))
        {
            ProcessStack(formatted.Substring(StackPrefix.Length).TrimStart(), keepEmptyEntries, out targets, out newArgs);
            return true;
        }

        if (formatted.StartsWith(SpectatedPrefix, StringComparison.OrdinalIgnoreCase))
        {
            GetSpectated(formatted.Substring(SpectatedPrefix.Length).TrimStart(), keepEmptyEntries, out targets, out newArgs);
            return true;
        }

        if (formatted.StartsWith(SpectatedPrefixShort, StringComparison.OrdinalIgnoreCase))
        {
            GetSpectated(formatted.Substring(SpectatedPrefixShort.Length).TrimStart(), keepEmptyEntries, out targets, out newArgs);
            return true;
        }

        targets = null;
        newArgs = null;
        return false;
    }

    private static void GetSpectated(string formatted, bool keepEmptyEntries, out List<ReferenceHub> targets, out string[] newArgs)
    {
        if (CurrentSender is not PlayerCommandSender {ReferenceHub: {roleManager.CurrentRole: SpectatorRole} hub})
        {
            targets = HubCollection.Empty;
            newArgs = Split(formatted, keepEmptyEntries, true);
            return;
        }

        foreach (var target in AllPlayers)
        {
            if (!target.IsSpectatedBy(hub))
                continue;
            targets = new HubCollection(target);
            newArgs = Split(formatted, keepEmptyEntries);
            return;
        }

        targets = HubCollection.Empty;
        newArgs = Split(formatted, keepEmptyEntries);
    }

    private static void ProcessStack(string formatted, bool keepEmptyEntries, out List<ReferenceHub> targets, out string[] newArgs)
    {
        if (CurrentSender == null)
        {
            targets = HubCollection.Empty;
            newArgs = Split(formatted, keepEmptyEntries);
            return;
        }

        var stack = PlayerSelectionStack.Get(CurrentSender);
        if (formatted.Length < 1)
        {
            targets = stack.IsEmpty ? HubCollection.Empty : stack.Pop();
            newArgs = Array.Empty<string>();
            return;
        }

        var sub = Split(formatted, keepEmptyEntries);
        if (formatted.IndexOfAny(StackSeparators) != 0)
        {
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

    private static List<ReferenceHub> GetStackTargets(PlayerSelectionStack stack, string options)
    {
        if (stack.IsEmpty)
            return HubCollection.Empty;
        return options.ToLower() switch
        {
            "first" or "f" => stack.Pop(),
            "last" or "l" => stack.PopAt(stack.LastIndex),
            "all" or "a" or "*" => stack.PopAll(),
            _ => ParseNumericStackOptions(stack, options.ToLowerInvariant())
        };
    }

    private static List<ReferenceHub> ParseNumericStackOptions(PlayerSelectionStack stack, string options)
        => !int.TryParse(options, out var index) || index < 0 || index >= stack.Count
            ? HubCollection.Empty
            : stack.PopAt(index);

    /// <summary>
    /// Splits the given string by spaces, optionally keeping empty entries.
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <param name="keepEmptyEntries">Whether to keep empty entries.</param>
    /// <param name="preTrimStart">Whether to trim the whitespace character at index 0.</param>
    /// <returns>The split string.</returns>
    public static string[] Split(string s, bool keepEmptyEntries, bool preTrimStart = false)
    {
        if (preTrimStart && s.Length > 0 && char.IsWhiteSpace(s[0]))
            s = s.Substring(1);
        return s.Split(new[] {' '}, keepEmptyEntries ? StringSplitOptions.None : StringSplitOptions.RemoveEmptyEntries);
    }

}
