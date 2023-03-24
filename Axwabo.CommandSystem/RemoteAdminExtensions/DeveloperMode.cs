using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Attributes.RaExt;
using Axwabo.CommandSystem.Patches;
using Axwabo.CommandSystem.Structs;
using Axwabo.Helpers;
using CommandSystem;
using RemoteAdmin;
using RemoteAdmin.Communication;

namespace Axwabo.CommandSystem.RemoteAdminExtensions;

/// <summary>A Remote Admin option to query the last exception and executed command.</summary>
[RemoteAdminOptionProperties("devMode", "Developer Mode")]
public sealed class DeveloperMode : ButtonBasedRemoteAdminOption
{

    public static readonly string CopyPlayerId = "<link=CP_ID>\uF0C5</link>".Color("green");

    public static readonly string CopyUserId = "<link=CP_USERID>\uF0C5</link>".Color("green");

    private static readonly Dictionary<ICommandSender, (string, Exception)> LastExceptions = new();

    private static readonly Dictionary<ICommandSender, (string, CommandResult)> LastCommands = new();

    public static void OnExceptionThrown(ICommandSender sender, string[] query, Exception exception)
    {
        if (sender is PlayerCommandSender player)
            LastExceptions[player] = (string.Join(" ", query), exception);
    }

    public static void OnCommandExecuted(ICommandSender sender, string[] query, string result, bool success)
    {
        if (sender is PlayerCommandSender player)
            LastCommands[player] = (string.Join(" ", query), new CommandResult(success, result));
    }

    protected override string OnBasicInfoClicked(PlayerCommandSender sender)
    {
        if (!LastCommands.TryGetValue(sender, out var data))
            return "You have not yet executed a command.".Color("red");
        RaClipboard.Send(sender, RaClipboard.RaClipBoardType.PlayerId, data.Item1);
        RaClipboard.Send(sender, RaClipboard.RaClipBoardType.UserId, data.Item2);
        return $"{CopyPlayerId}{data.Item1.Color(data.Item2 ? "green" : "orange")}\n{CopyUserId}{data.Item2}";
    }

    protected override string OnRequestIPClicked(PlayerCommandSender sender) => HandleException(sender, false);

    protected override string OnRequestAuthClicked(PlayerCommandSender sender) => HandleException(sender, true);

    private static string HandleException(CommandSender sender, bool includeOffsets)
    {
        if (!LastExceptions.TryGetValue(sender, out var data))
            return "No exceptions have been thrown yet by commands you executed.".Color("red");
        var stackTrace = includeOffsets ? data.Item2.ToString() : RemoveStackTraceZeroesPatch.StripILOffsets(data.Item2.ToString());
        RaClipboard.Send(sender, RaClipboard.RaClipBoardType.PlayerId, data.Item1);
        RaClipboard.Send(sender, RaClipboard.RaClipBoardType.UserId, stackTrace);
        return $"{CopyPlayerId}{data.Item1.Color("orange")}\n{CopyUserId}{stackTrace}";
    }

}
