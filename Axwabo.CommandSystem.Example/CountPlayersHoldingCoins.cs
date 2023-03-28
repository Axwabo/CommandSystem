using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Permissions;
using Utils.NonAllocLINQ;

// Northwood's LINQ extension methods

namespace Axwabo.CommandSystem.Example;

[CommandProperties(CommandHandlerType.RemoteAdmin, "cphc")]
[OneOfVanillaPermissions(PlayerPermissions.GameplayData, PlayerPermissions.PlayersManagement)] // at least one of these permissions is required
public sealed class CountPlayersHoldingCoins : UnifiedTargetingCommand
{

    protected override CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender)
    {
        var count = targets.Count(p => p.inventory.CurItem.TypeId == ItemType.Coin);
        return count == 0
            ? "No players holding coins found"
            // use the extension method from Axwabo.CommandSystem.Extensions
            // pluralize the word "player" depending on the count, e.g. "1 player" or "2 players"
            : $"Found {"player".PluralizeWithCount(count)} holding a coin";
    }

}
