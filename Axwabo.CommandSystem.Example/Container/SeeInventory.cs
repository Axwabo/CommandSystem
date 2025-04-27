using System;
using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Extensions;
using Axwabo.CommandSystem.Permissions;
using LabApi.Features.Wrappers;

namespace Axwabo.CommandSystem.Example.Container;

[CommandName("see")]
[VanillaPermissions(PlayerPermissions.GameplayData)]
public sealed class SeeInventory : SeparatedTargetingCommand, ICustomResultCompiler
{

    protected override CommandResult ExecuteOn(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender)
    {
        var player = Player.Get(target);
        if (player.IsWithoutItems)
            return false;
        return string.Join(", ", player.Items.Select(e => e!.Type));
    }

    public CommandResult? CompileResultCustom(List<CommandResultOnTarget> success, List<CommandResultOnTarget> failures)
    {
        if (success.Count == 0)
            return ("No targets have any items.", false); // tuples (string, bool) and (bool, string) implicitly cast to CommandResult
        return (true, success.JoinResults());
    }

}
