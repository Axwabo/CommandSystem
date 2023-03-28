using System;
using Axwabo.Helpers;
using PluginAPI.Core;

namespace Axwabo.CommandSystem.Example.Resolvers;

[EnumCommand(CustomCommandType.DiscordBroadcast)]
public sealed class DiscordBroadcastCommand : CommandBase
{

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        Server.SendBroadcast("Join our Discord server!".Size(70) + "\nLink in the server info.", 15);
        return "Broadcast sent.";
    }

}
