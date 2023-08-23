using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Attributes.Containers;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Permissions;
using InventorySystem.Disarming;

namespace Axwabo.CommandSystem.Example.Container;

[CommandProperties(CommandHandlerType.RemoteAdmin, "inventory")]
// this container registers two subcommands, see and drop
// the equip command is a subcommand of this, specified in its class
[UsesSubcommands(typeof(SeeInventory), typeof(DropItem))]
public sealed class InventoryCommand : ContainerCommand
{

    // register this method as a subcommand
    [MethodBasedSubcommand]
    [VanillaPermissions(PlayerPermissions.PlayersManagement)]
    public CommandResult GetDisarmer(ArraySegment<string> arguments, CommandSender sender)
    {
        if (arguments.Count == 0)
            return "!Please specify a player";
        var targets = arguments.GetTargets(out _);
        if (targets.Count == 0)
            return "!No target found";
        var player = targets[0];
        var id = player.netId;
        foreach (var entry in DisarmedPlayers.Entries)
            if (entry.DisarmedPlayer == id && ReferenceHub.TryGetHubNetID(entry.Disarmer, out var disarmer))
                return $"!Player \"{player.nicknameSync.MyNick}\" is disarmed by \"{disarmer.nicknameSync.MyNick}\"";
        return $"Player \"{player.nicknameSync.MyNick}\" is not disarmed";
    }

}
