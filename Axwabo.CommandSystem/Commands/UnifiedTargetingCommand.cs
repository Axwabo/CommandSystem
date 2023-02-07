using System;
using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.Selectors;
using Axwabo.CommandSystem.Structs;
using PlayerRoles;

namespace Axwabo.CommandSystem.Commands;

public abstract class UnifiedTargetingCommand : CommandBase {

    private readonly string _noTargetsFoundMessage = "No targets were found.";
    private readonly string _noPlayersAffected = "No players were affected.";
    private readonly bool _shouldAffectSpectators = true;

    private readonly IAffectedMultiplePlayersMessageGenerator _affectedMultipleGenerator;
    private readonly IAffectedAllPlayersGenerator _allAffectedGenerator;
    private readonly IAffectedOnePlayerMessageGenerator _affectedOneGenerator;
    private readonly ITargetSelectionManager _selectionManager;

    protected UnifiedTargetingCommand() {
        var affectedMultiple = "Done! The request affected {0}.";
        var affectedOne = "Done! The request affected {0}.";
        TargetingCommandPropertyManager.ResolveProperties(this, ref _noTargetsFoundMessage, ref affectedMultiple, ref affectedOne, ref _noPlayersAffected, ref _shouldAffectSpectators);
        TargetingCommandPropertyManager.ResolveGenerators(this, out _affectedMultipleGenerator, out _allAffectedGenerator, out _affectedOneGenerator, out _selectionManager);
        if (_affectedMultipleGenerator is not null || _allAffectedGenerator is not null || _affectedOneGenerator is not null)
            return;
        var generator = new DefaultTargetingMessageGenerator {
            AffectedMultipleMessage = affectedMultiple,
            AffectedOneMessage = affectedOne
        };
        _affectedMultipleGenerator ??= generator;
        _allAffectedGenerator ??= generator;
        _affectedOneGenerator ??= generator;
    }

    protected override int MinArguments => base.MinArguments + 1;

    private bool ShouldBeAffected(ReferenceHub hub) => ShouldAffectSpectators || hub.IsAlive();

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender) {
        var targets = arguments.GetTargets(out var newArgs)?.Where(ShouldBeAffected).ToList();
        if (targets is not {Count: not 0})
            return OnNoTargetsFound();
        var args = new ArraySegment<string>(newArgs ?? Array.Empty<string>());
        return targets.Count == 1 ? ExecuteOnSingleTarget(targets[0], arguments, sender) : ExecuteOnTargets(targets, args, sender);
    }

    protected abstract CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender);

    protected virtual CommandResult ExecuteOnSingleTarget(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender)
        => ExecuteOnTargets(new List<ReferenceHub> {target}, arguments, sender);

    protected virtual CommandResult OnNoTargetsFound() => CommandResult.Failed(NoTargetsFoundMessage);

    protected virtual string NoTargetsFoundMessage => _noTargetsFoundMessage;

    protected virtual string NoPlayersAffected => _noPlayersAffected;

    protected bool ShouldAffectSpectators => _selectionManager?.AffectSpectators ?? _shouldAffectSpectators;

    protected bool IsEveryoneAffected(int affected)
        => _selectionManager?.IsEveryoneAffected(affected)
           ?? (ShouldAffectSpectators ? PlayerSelectionManager.AllPlayers : PlayerSelectionManager.NonSpectators).Count == affected;

    protected string GetAffectedMessage(int players) => _affectedMultipleGenerator.OnAffected(players);

    protected string GetAffectedMessageSingle(ReferenceHub target) => _affectedOneGenerator.OnAffected(target);

    protected string GetAffectedMessageAll(int players) => _allAffectedGenerator.OnEveryoneAffected(players);

}
