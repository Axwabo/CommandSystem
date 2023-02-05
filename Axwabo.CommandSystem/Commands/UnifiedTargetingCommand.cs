using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.Selectors;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Commands;

// ReSharper disable SuspiciousTypeConversion.Global
public abstract class UnifiedTargetingCommand : CommandBase {

    private readonly string _noTargetsFound = "No targets were found.";
    private readonly string _affected = "Done! The request affected {0}.";
    private readonly string _noPlayersAffected = "No players were affected.";
    private readonly bool _shouldAffectSpectators = true;

    protected UnifiedTargetingCommand()
        => TargetingCommandPropertyManager.ResolveProperties(this, ref _noTargetsFound, ref _affected, ref _noPlayersAffected, ref _shouldAffectSpectators);

    protected override int MinArguments => base.MinArguments + 1;

    protected sealed override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender) {
        var targets = arguments.GetTargets(out var newArgs);
        if (targets is not {Count: not 0})
            return CommandResult.Failed(NoTargetsFound);
        var args = (newArgs ?? Array.Empty<string>()).Segment(0);
        return targets.Count == 1 ? ExecuteOnSingleTarget(targets[0], arguments, sender) : ExecuteOnTargets(targets, args, sender);
    }

    protected abstract CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender);

    protected virtual CommandResult ExecuteOnSingleTarget(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender)
        => ExecuteOnTargets(new List<ReferenceHub> {target}, arguments, sender);

    protected virtual string NoTargetsFound => _noTargetsFound;

    protected string NoPlayersAffected => this is INoPlayersAffected np ? np.NoPlayersAffected : _noPlayersAffected;

    protected bool AffectSpectators => this is ITargetSelectionManager mgr ? mgr.AffectSpectators : _shouldAffectSpectators;

    protected string GetAffectedMessage(int players)
        => this is IAffectedMessage msg ? msg.OnAffected(players) : string.Format(_affected, "player".Pluralize(players));

    protected bool IsEveryoneAffected(int affected)
        => this is ITargetSelectionManager msg
            ? msg.IsEveryoneAffected(affected)
            : (AffectSpectators ? PlayerSelectionManager.AllPlayers : PlayerSelectionManager.NonSpectators).Count == affected;

    protected string GetAffectedMessageAll(int players)
        => this is IAffectedAllPlayers msg
            ? msg.OnEveryoneAffected(players)
            : GetAffectedMessage(players);

    protected string GetAffectedMessageSingle(ReferenceHub target) => this is IAffectedOnePlayer msg
        ? msg.OnAffected(target)
        : string.Format(_affected, target.nicknameSync.MyNick);

}
