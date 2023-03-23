using System;
using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.Structs;
using Axwabo.Helpers;

namespace Axwabo.CommandSystem.Example.Container;

// specified as a subcommand in the InventoryCommand container
[CommandName("drop")]
[Usage("<item>")]
[MinArguments(1)]
[OneOfVanillaPermissions(PlayerPermissions.GivingItems, PlayerPermissions.PlayersManagement)]
public sealed class DropItem : UnifiedTargetingCommand
{

    protected override CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender)
    {
        if (!Parse.Item(arguments.At(0), out var item))
            return "!Invalid item id.";
        var affected = 0;
        foreach (var hub in targets)
        {
            var instance = hub.inventory.UserInventory.Items.Values.FirstOrDefault(e => e.ItemTypeId == item);
            if (instance == null)
                continue;
            hub.inventory.UserCode_CmdDropItem(instance.ItemSerial, false);
            affected++;
        }

        return affected == 0 ? "!No targets had the item." : $"Dropped {item} from {"player".PluralizeWithCount(affected)}.";
    }

}
