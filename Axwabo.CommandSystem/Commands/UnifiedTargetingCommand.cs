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

    private readonly TargetingCommandProperties _properties;

    private readonly IAffectedOnePlayerMessageGenerator _affectedOneGenerator;
    private readonly IAffectedMultiplePlayersMessageGenerator _affectedMultipleGenerator;
    private readonly IAffectedAllPlayersMessageGenerator _affectedAllGenerator;

    /// <summary>
    /// Creates a new <see cref="UnifiedTargetingCommand"/> instance.
    /// Properties are resolved based on the type.
    /// </summary>
    protected UnifiedTargetingCommand() : this(null)
    {
    }

    /// <summary>
    /// Creates a new <see cref="UnifiedTargetingCommand"/> instance based on the supplied <see cref="TargetingCommandProperties">properties</see>.
    /// If <paramref name="properties"/> is null, <see cref="TargetingCommandPropertyManager.ResolveProperties"/> will be invoked to get properties.
    /// If the <see cref="TargetingCommandProperties.BaseProperties"/> pf <paramref name="properties"/> is null, <see cref="BaseCommandPropertyManager.ResolveProperties"/> will be invoked to get properties.
    /// </summary>
    protected UnifiedTargetingCommand(TargetingCommandProperties properties) : base(properties?.BaseProperties)
    {
        _properties = properties?.Clone() ?? TargetingCommandPropertyManager.ResolveProperties(GetType(), this);
        _properties.SelectionManager.SafeCastAndSetIfNull(ref _properties.FilteringPolicy);
        var defaultGenerator = new DefaultTargetingMessageGenerator(_properties.AffectedOneMessage, _properties.AffectedMultipleMessage);
        _affectedOneGenerator = _properties.AffectedOneGenerator ?? defaultGenerator;
        _affectedMultipleGenerator = _properties.AffectedMultipleGenerator ?? defaultGenerator;
        _affectedAllGenerator = _properties.AffectedAllGenerator ?? defaultGenerator;
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
           && (_properties.FilteringPolicy?.FilterTarget(hub) ?? true);

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
    protected virtual string NoTargetsFoundMessage => _properties.NoTargetsFoundMessage;

    /// <summary>Gets the raw message to display when no players were affected without formatting.</summary>
    protected virtual string NoPlayersAffected => _properties.NoPlayersAffectedMessage;

    /// <summary>Whether the command should affect spectators.</summary>
    protected bool ShouldAffectSpectators => _properties.SelectionManager?.AffectSpectators ?? _properties.ShouldAffectSpectators;

    /// <summary>
    /// Checks if everyone was affected based on the number of affected players.
    /// </summary>
    /// <param name="affected">The number of affected players.</param>
    /// <returns>Whether everyone was affected.</returns>
    protected bool IsEveryoneAffectedInternal(int affected)
        => _properties.SelectionManager?.IsEveryoneAffected(affected)
           ?? (_properties.ShouldAffectSpectators ? PlayerSelectionManager.AllPlayers : PlayerSelectionManager.NonSpectators).Count(ShouldBeAffected) == affected;

    /// <summary>
    /// Formats the message to display when a single player was affected.
    /// </summary>
    /// <param name="target">The affected player.</param>
    /// <returns>The formatted message.</returns>
    protected string GetAffectedMessageSingle(ReferenceHub target) => _affectedOneGenerator.OnAffected(target);

    /// <summary>
    /// Formats the message to display when more than one player was affected.
    /// </summary>
    /// <param name="players">The number of affected players.</param>
    /// <returns>The formatted message.</returns>
    protected string GetAffectedMessage(int players) => _affectedMultipleGenerator.OnAffected(players);

    /// <summary>
    /// Formats the message to display when everyone was affected.
    /// </summary>
    /// <param name="players">The number of affected players.</param>
    /// <returns>The formatted message.</returns>
    protected string GetAffectedMessageAll(int players) => _affectedAllGenerator.OnEveryoneAffected(players);

}
