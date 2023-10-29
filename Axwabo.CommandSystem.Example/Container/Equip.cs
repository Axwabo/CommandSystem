using System;
using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Attributes.Containers;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Permissions;
using Axwabo.Helpers;

namespace Axwabo.CommandSystem.Example.Container;

// instead of specifying the subcommands in the container, we let the container know that this is a subcommand of it
[SubcommandOfContainer(typeof(InventoryCommand))]
[CommandName("equip")]
[Usage("<item>")]
[MinArguments(1)]
[AllVanillaPermissions(PlayerPermissions.GivingItems, PlayerPermissions.PlayersManagement)]
public sealed class Equip : UnifiedTargetingCommand
{

    protected override CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender)
    {
        if (!arguments.ParseItem(out var item, includeNone: true))
            return "!Invalid item id.";
        var affected = 0;
        foreach (var hub in targets)
        {
            if (item == ItemType.None)
            {
                affected += hub.inventory.CurItem.TypeId != ItemType.None ? 1 : 0;
                hub.inventory.ServerSelectItem(0);
                continue;
            }

            var instance = hub.inventory.UserInventory.Items.Values.FirstOrDefault(e => e.ItemTypeId == item);
            if (instance == null)
                continue;
            hub.inventory.ServerSelectItem(instance.ItemSerial);
            affected++;
        }

        if (affected == 0)
            return "!No players affected.";
        return item == ItemType.None
            ? $"Unequipped {"player".PluralizeWithCount(affected)}."
            : $"Equipped {item} on {"player".PluralizeWithCount(affected)}.";
    }

}
