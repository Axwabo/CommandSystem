using System;
using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.Selectors;
using Axwabo.CommandSystem.Structs;
using PlayerRoles;

namespace Axwabo.CommandSystem.Commands;

// ReSharper disable SuspiciousTypeConversion.Global
public abstract class UnifiedTargetingCommand : CommandBase {

    private readonly string _noTargetsFound = "No targets were found.";
    private readonly string _affectedMultiple = "Done! The request affected {0}.";
    private readonly string _affectedSingle = "Done! The request affected {0}.";
    private readonly string _noPlayersAffected = "No players were affected.";
    private readonly bool _shouldAffectSpectators = true;

    private readonly IAffectedMultiplePlayersMessageGenerator _affectedMultiplePlayersGenerator;
    private readonly IAffectedAllPlayersGenerator _allAffectedGenerator;
    private readonly IAffectedOnePlayerGenerator _affectedOneGenerator;
    private readonly ITargetSelectionManager _selectionManager;
    private readonly INoPlayersAffectedGenerator _noPlayersAffectedGenerator;

    protected UnifiedTargetingCommand() {
        TargetingCommandPropertyManager.ResolveProperties(this, ref _noTargetsFound, ref _affectedMultiple, ref _affectedSingle, ref _noPlayersAffected, ref _shouldAffectSpectators);
        _affectedMultiplePlayersGenerator = this as IAffectedMultiplePlayersMessageGenerator;
        _allAffectedGenerator = this as IAffectedAllPlayersGenerator;
        _affectedOneGenerator = this as IAffectedOnePlayerGenerator;
        _selectionManager = this as ITargetSelectionManager;
        _noPlayersAffectedGenerator = this as INoPlayersAffectedGenerator;
    }

    protected override int MinArguments => base.MinArguments + 1;

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender) {
        var targets = arguments.GetTargets(out var newArgs);
        if (targets is not {Count: not 0})
            return CommandResult.Failed(NoTargetsFound);
        var args = (newArgs ?? Array.Empty<string>()).Segment(0);
        if (targets.Count != 1)
            return ExecuteOnTargets(targets.Where(PlayerRolesUtils.IsAlive).ToList(), args, sender);
        return targets[0].IsAlive()
            ? ExecuteOnSingleTarget(targets[0], arguments, sender)
            : CommandResult.Failed(NoTargetsFound);
    }

    protected abstract CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender);

    protected virtual CommandResult ExecuteOnSingleTarget(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender)
        => ExecuteOnTargets(new List<ReferenceHub> {target}, arguments, sender);

    protected virtual string NoTargetsFound => _noTargetsFound;

    protected string NoPlayersAffected => _noPlayersAffectedGenerator?.NoPlayersAffected ?? _noPlayersAffected;

    protected bool AffectSpectators => _selectionManager?.AffectSpectators ?? _shouldAffectSpectators;

    protected bool IsEveryoneAffected(int affected)
        => _selectionManager?.IsEveryoneAffected(affected)
           ?? (AffectSpectators ? PlayerSelectionManager.AllPlayers : PlayerSelectionManager.NonSpectators).Count == affected;

    protected string GetAffectedMessage(int players)
        => _affectedMultiplePlayersGenerator != null
            ? _affectedMultiplePlayersGenerator.OnAffected(players)
            : string.Format(_affectedMultiple, "player".Pluralize(players));

    protected string GetAffectedMessageSingle(ReferenceHub target)
        => _affectedOneGenerator != null
            ? _affectedOneGenerator.OnAffected(target)
            : string.Format(_affectedSingle, target.nicknameSync.MyNick);

    protected string GetAffectedMessageAll(int players)
        => _allAffectedGenerator != null
            ? _allAffectedGenerator.OnEveryoneAffected(players)
            : GetAffectedMessage(players);

}
