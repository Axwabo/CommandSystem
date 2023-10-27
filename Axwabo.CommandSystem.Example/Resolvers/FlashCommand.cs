using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.Selectors;
using CustomPlayerEffects;
using PlayerRoles;

namespace Axwabo.CommandSystem.Example.Resolvers;

[EnumCommand(CustomCommandType.Flash)]
public sealed class FlashCommand : SeparatedTargetingCommand, ITargetSelectionManager, ICustomResultCompiler
{

    public bool AffectSpectators => true; // let's do a funny: make spectators targets but make the command fail

    public bool IsEveryoneAffected(int count) => PlayerSelectionManager.AllPlayers.Count == count;

    protected override CommandResult ExecuteOn(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender)
    {
        if (!target.IsAlive())
            return false;
        target.playerEffectsController.EnableEffect<Flashed>(10);
        return true;
    }

    public CommandResult? CompileResultCustom(List<CommandResultOnTarget> success, List<CommandResultOnTarget> failures)
    {
        var successes = success.Count > 0 ? $"Flashed the following players for 10 seconds: {success.CombineNicknames()}" : "";
        var failed = failures.Count > 0 ? $"Unaffected: {failures.CombineNicknames()}" : "";
        return new CommandResult(successes.Length > 0, $"{successes}\n{failed}".Trim()); // remove unnecessary newline
    }

}
