using System;
using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Permissions;
using Axwabo.Helpers;

namespace Axwabo.CommandSystem.Example.Container;

// specified as a subcommand in the InventoryCommand container
[CommandName("drop")]
[Usage("<item>")]
[MinArguments(1)]
[OneOfVanillaPermissions(PlayerPermissions.GivingItems, PlayerPermissions.PlayersManagement)]
public sealed class DropItem : SeparatedTargetingCommand, ITargetingPreExecutionFilter, ICustomResultCompiler
{

    private ItemType _item;

    public CommandResult? OnBeforeExecuted(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender)
    {
        if (arguments.Count < 1)
            return CommandResult.Null; // base class will handle absence of arguments
        if (!arguments.ParseItem(out _item))
            return "!Invalid item id.";
        return CommandResult.Null; // allow execution
    }

    protected override CommandResult ExecuteOn(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender)
    {
        var instance = target.inventory.UserInventory.Items.Values.FirstOrDefault(e => e.ItemTypeId == _item);
        if (instance == null)
            return false;
        target.inventory.UserCode_CmdDropItem__UInt16__Boolean(instance.ItemSerial, false);
        return true;
    }

    public CommandResult? CompileResultCustom(List<CommandResultOnTarget> success, List<CommandResultOnTarget> failures)
        => success.Count == 0
            ? "!No targets had the item."
            : $"Dropped {_item} from {"player".PluralizeWithCount(success.Count)}.";

}
