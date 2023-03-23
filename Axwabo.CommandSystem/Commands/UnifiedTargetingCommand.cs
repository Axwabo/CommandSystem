using System;
using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Attributes.Targeting.Interfaces;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.Selectors;
using Axwabo.CommandSystem.Structs;
using PlayerRoles;

namespace Axwabo.CommandSystem.Commands;

/// <summary>
/// A base class for handling a command which accepts a list of targets as the first argument.
/// This class executes the command at once with a <see cref="List{T}">player list</see>.
/// </summary>
/// <seealso cref="SeparatedTargetingCommand"/>
public abstract class UnifiedTargetingCommand : CommandBase
{

    private static readonly string[] Players = {"<players>"};

    private readonly string _noTargetsFoundMessage = "No targets were found.";
    private readonly string _noPlayersAffected = "No players were affected.";
    private readonly bool _shouldAffectSpectators;

    private readonly IAffectedMultiplePlayersMessageGenerator _affectedMultipleGenerator;
    private readonly IAffectedAllPlayersMessageGenerator _allAffectedGenerator;
    private readonly IAffectedOnePlayerMessageGenerator _affectedOneGenerator;
    private readonly ITargetSelectionManager _selectionManager;

    /// <summary>
    /// Creates a new <see cref="UnifiedTargetingCommand"/> instance.
    /// </summary>
    protected UnifiedTargetingCommand()
    {
        _shouldAffectSpectators = this is not IShouldAffectSpectators {AffectSpectators: false};
        var affectedMultiple = "Done! The request affected {0}.";
        var affectedOne = affectedMultiple;
        TargetingCommandPropertyManager.ResolveProperties(this, ref _noTargetsFoundMessage, ref affectedMultiple, ref affectedOne, ref _noPlayersAffected, ref _shouldAffectSpectators);
        TargetingCommandPropertyManager.ResolveGenerators(this, out _affectedMultipleGenerator, out _allAffectedGenerator, out _affectedOneGenerator, out _selectionManager);
        if (_affectedMultipleGenerator is not null && _allAffectedGenerator is not null && _affectedOneGenerator is not null)
            return;
        var generator = new DefaultTargetingMessageGenerator
        {
            AffectedMultipleMessage = affectedMultiple,
            AffectedOneMessage = affectedOne
        };
        _affectedMultipleGenerator ??= generator;
        _allAffectedGenerator ??= generator;
        _affectedOneGenerator ??= generator;
    }

    /// <inheritdoc />
    protected sealed override int MinArguments => MinArgumentsWithoutTargets + 1;

    /// <summary>The minimum amount of arguments required to execute the command excluding the player list argument.</summary>
    protected virtual int MinArgumentsWithoutTargets => base.MinArguments;

    /// <inheritdoc />
    public override string[] Usage => base.Usage is {Length: not 0} usage
        ? usage.Select(e => $"<players> {e}").ToArray()
        : Players;

    private bool ShouldBeAffected(ReferenceHub hub)
        => (ShouldAffectSpectators || hub.IsAlive())
           && (this is not ITargetFilteringPolicy policy || policy.FilterTarget(hub));

    /// <inheritdoc />
    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        if (arguments.Count < 1)
            return OnNotEnoughArguments(arguments, sender, MinArguments);
        var targets = arguments.GetTargets(out var newArgs)?.Where(ShouldBeAffected).ToList();
        var args = new ArraySegment<string>(newArgs ?? Array.Empty<string>());
        if (targets is not {Count: not 0})
            return OnNoTargetsFound(arguments, args, sender);
        return args.Count < MinArgumentsWithoutTargets
            ? OnNotEnoughArguments(args, sender, MinArguments)
            : targets.Count == 1
                ? ExecuteOnSingleTarget(targets[0], args, sender)
                : ExecuteOnTargets(targets, args, sender);
    }

    /// <summary>
    /// Executes the command on the given targets.
    /// </summary>
    /// <param name="targets">The targets to execute the command on.</param>
    /// <param name="arguments">The arguments passed to the command excluding the player list.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <returns>The result of the command execution.</returns>
    protected abstract CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender);

    /// <summary>
    /// Executes the command on a single target. Only called when there is exactly one target.
    /// </summary>
    /// <param name="target">The target to execute the command on.</param>
    /// <param name="arguments">The arguments passed to the command excluding the player list.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <returns>The result of the command execution.</returns>
    protected virtual CommandResult ExecuteOnSingleTarget(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender)
        => ExecuteOnTargets(new List<ReferenceHub> {target}, arguments, sender);

    /// <summary>
    /// Called when no targets were found.
    /// </summary>
    /// <param name="originalArguments">The original arguments passed to the command.</param>
    /// <param name="postParsingArguments">The arguments passed to the command excluding the player list.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <returns>The result of the command execution.</returns>
    protected virtual CommandResult OnNoTargetsFound(ArraySegment<string> originalArguments, ArraySegment<string> postParsingArguments, CommandSender sender)
        => CommandResult.Failed(NoTargetsFoundMessage);

    /// <summary>Gets the raw message to display when no targets were found without formatting.</summary>
    protected virtual string NoTargetsFoundMessage => _noTargetsFoundMessage;

    /// <summary>Gets the raw message to display when no players were affected without formatting.</summary>
    protected virtual string NoPlayersAffected => _noPlayersAffected;

    /// <summary>Whether the command should affect spectators.</summary>
    protected bool ShouldAffectSpectators => _selectionManager?.AffectSpectators ?? _shouldAffectSpectators;

    /// <summary>
    /// Checks if everyone was affected based on the number of affected players.
    /// </summary>
    /// <param name="affected">The number of affected players.</param>
    /// <returns>Whether everyone was affected.</returns>
    protected bool IsEveryoneAffectedInternal(int affected)
        => _selectionManager?.IsEveryoneAffected(affected)
           ?? (ShouldAffectSpectators ? PlayerSelectionManager.AllPlayers : PlayerSelectionManager.NonSpectators).Count == affected;

    /// <summary>
    /// Formats the message to display when more than one player was affected.
    /// </summary>
    /// <param name="players">The number of affected players.</param>
    /// <returns>The formatted message.</returns>
    protected string GetAffectedMessage(int players) => _affectedMultipleGenerator.OnAffected(players);

    /// <summary>
    /// Formats the message to display when a single player was affected.
    /// </summary>
    /// <param name="target">The affected player.</param>
    /// <returns>The formatted message.</returns>
    protected string GetAffectedMessageSingle(ReferenceHub target) => _affectedOneGenerator.OnAffected(target);

    /// <summary>
    /// Formats the message to display when everyone was affected.
    /// </summary>
    /// <param name="players">The number of affected players.</param>
    /// <returns>The formatted message.</returns>
    protected string GetAffectedMessageAll(int players) => _allAffectedGenerator.OnEveryoneAffected(players);

}
