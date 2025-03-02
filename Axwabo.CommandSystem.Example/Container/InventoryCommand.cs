using System;
using System.Collections.Generic;
using System.ComponentModel;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Attributes.Containers;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Permissions;
using InventorySystem.Disarming;

namespace Axwabo.CommandSystem.Example.Container;

[CommandProperties(CommandHandlerType.RemoteAdmin, "inventory")]
// this container registers two subcommands, see and drop
// the equip command is a subcommand of this, specified by a different attribute on its class
[UsesSubcommands(typeof(SeeInventory), typeof(DropItem))]
public sealed class InventoryCommand : ContainerCommand
{

    // register this method as a subcommand
    // automatically detects targeting commands: insert first param of type List<ReferenceHub>
    [MethodBasedSubcommand]
    [VanillaPermissions(PlayerPermissions.PlayersManagement)]
    [Description("Gets the player who disarmed the target.")]
    // if the command name is not specified, the method name is used
    public CommandResult GetDisarmer(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender)
    {
        var player = targets[0];
        var id = player.netId;
        foreach (var entry in DisarmedPlayers.Entries)
            if (entry.DisarmedPlayer == id && ReferenceHub.TryGetHubNetID(entry.Disarmer, out var disarmer))
                return $"!Player \"{player.nicknameSync.MyNick}\" is disarmed by \"{disarmer.nicknameSync.MyNick}\"";
        return $"Player \"{player.nicknameSync.MyNick}\" is not disarmed";
    }

}
