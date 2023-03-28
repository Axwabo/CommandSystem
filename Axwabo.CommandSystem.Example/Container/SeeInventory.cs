using System;
using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.Example.Container;

[CommandName("see")]
[VanillaPermissions(PlayerPermissions.GameplayData)]
public sealed class SeeInventory : SeparatedTargetingCommand, ICustomResultCompiler
{

    protected override CommandResult ExecuteOn(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender)
    {
        var items = target.inventory.UserInventory.Items.Values;
        if (items.Count == 0)
            return false;
        return $"Inventory contents of {target.nicknameSync.MyNick}: " + string.Join(", ", items.Select(e => e.ItemTypeId));
    }

    public CommandResult CompileResultCustom(List<CommandResultOnTarget> success, List<CommandResultOnTarget> failures)
    {
        if (success.Count == 0)
            return ("No targets have any items.", false); // tuples <string, bool> and <bool, string> implicitly cast to CommandResult
        return (true, success.JoinResults("\n"));
    }

}
