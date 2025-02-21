using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.Extensions;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.Selectors;
using PlayerRoles;

namespace Axwabo.CommandSystem.Commands;

/// <summary>
/// A base class for handling a command which accepts a list of targets as the first argument.
/// This class executes the command at once with a <see cref="List{T}">player list</see>.
/// </summary>
/// <seealso cref="SeparatedTargetingCommand"/>
public abstract class UnifiedTargetingCommand : CommandBase
{

    private static readonly string[] Players = ["<players>"];

    private readonly string _noTargetsFoundMessage = "No targets were found.";
    private readonly string _noPlayersAffected = "No players were affected.";
    private readonly bool _shouldAffectSpectators;

    private readonly IAffectedMultiplePlayersMessageGenerator _affectedMultipleGenerator;
    private readonly IAffectedAllPlayersMessageGenerator _allAffectedGenerator;
    private readonly IAffectedOnePlayerMessageGenerator _affectedOneGenerator;
    private readonly ITargetSelectionManager _selectionManager;

    /// <summary>
    /// Creates a new <see cref="UnifiedTargetingCommand"/> instance.
    /// Properties are resolved based on the type.
    /// </summary>
    protected UnifiedTargetingCommand() : this(null)
    {
    }

    /// <summary>
    /// Creates a new <see cref="UnifiedTargetingCommand"/> instance based on the supplied <see cref="BaseCommandProperties">properties</see>.
    /// If <paramref name="properties"/> is null, <see cref="BaseCommandPropertyManager.ResolveProperties"/> will be invoked to get properties.
    /// </summary>
    protected UnifiedTargetingCommand(BaseCommandProperties properties) : base(properties)
    {
        _shouldAffectSpectators = this is not IShouldAffectSpectators {AffectSpectators: false};
        var affectedMultiple = DefaultTargetingMessageGenerator.DefaultAffectedMessage;
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
    public sealed override string[] Usage => UsageWithoutPlayers is {Length: not 0} usage
        ? usage.Select(e => $"<players> {e}").ToArray()
        : Players;

    /// <summary>The usage without the "&lt;players&gt;" prefixes.</summary>
    protected virtual string[] UsageWithoutPlayers => base.Usage;

    private bool ShouldBeAffected(ReferenceHub hub)
        => (ShouldAffectSpectators || hub.IsAlive())
           && ((_selectionManager ?? this as ITargetFilteringPolicy)?.FilterTarget(hub) ?? true);

    /// <inheritdoc />
    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        if (arguments.Count < 1)
            return OnNotEnoughArguments(arguments, sender, MinArguments);
        var targets = (arguments.GetTargets(out var newArgs)?.Where(ShouldBeAffected)).AsNonNullEnumerable().ToList();
        var args = new ArraySegment<string>(newArgs ?? []);
        return (this as ITargetingPreExecutionFilter)?.OnBeforeExecuted(targets, args, sender)
               ?? (targets is not {Count: not 0}
                   ? OnNoTargetsFound(arguments, args, sender)
                   : args.Count < MinArgumentsWithoutTargets
                       ? OnNotEnoughArguments(arguments, sender, MinArguments)
                       : ExecuteOnTargets(targets, args, sender));
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
           ?? (_shouldAffectSpectators ? PlayerSelectionManager.AllPlayers : PlayerSelectionManager.NonSpectators).Count(ShouldBeAffected) == affected;

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
